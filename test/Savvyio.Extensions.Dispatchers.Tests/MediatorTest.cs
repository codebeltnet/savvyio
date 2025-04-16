using System;
using System.Linq;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.Xunit.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.EventDriven;
using Savvyio.Assets.Queries;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dispatchers
{
    public class MediatorTest : Test
    {
        public MediatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Host_MediatorShouldBeRegistered_UsingDefaultOptions()
        {
            using (var test = HostTestFactory.Create(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>())))
            {
                var mediator = test.Host.Services.GetRequiredService<IMediator>();

                Assert.IsType<Mediator>(mediator);
            }
        }

        [Fact]
        public void Host_MediatorDescriptorShouldNotBeRegistered_UsingDefaultOptions()
        {
            using var test = HostTestFactory.Create(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>()));
            Assert.Throws<InvalidOperationException>(() => test.Host.Services.GetRequiredService<IHandlerServicesDescriptor>());
        }

        [Fact]
        public void Host_MediatorDescriptorShouldBeRegistered()
        {
            using var test = HostTestFactory.Create(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>().EnableHandlerServicesDescriptor()).AddHandlerServicesDescriptor());
            var descriptor = test.Host.Services.GetRequiredService<IHandlerServicesDescriptor>();

            Assert.IsType<HandlerServicesDescriptor>(descriptor);

            TestOutput.WriteLine(descriptor.ToString());
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreateAccountAsync_OnInProcAccountCreated_OnOutProcAccountCreated()
        {
            await using var test = HostTestFactory.Create(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddEfCoreRepository<Account, long, Account>();
                services.AddEfCoreDataStore<PlatformProvider, PlatformProvider>();
                services.AddDataSource<CustomEfCoreDataSource>()
                    .AddUnitOfWork<CustomEfCoreDataSource>();
                services.AddEfCoreDataSource<PlatformProvider>(o =>
                {
                    o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                    o.ModelConstructor = mb => mb.AddPlatformProvider();
                });
                services.AddSavvyIO(o => o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true).AddMediator<Mediator>());
                services.AddHandlerServicesDescriptor();
                services.AddSingleton<ITestStore<IDomainEvent>, InMemoryTestStore<IDomainEvent>>();
                services.AddSingleton<ITestStore<IIntegrationEvent>, InMemoryTestStore<IIntegrationEvent>>();
            });
            using var scope = test.Host.Services.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var descriptor = scope.ServiceProvider.GetRequiredService<IHandlerServicesDescriptor>();
            var deStore = scope.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
            var ieStore = scope.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

            TestOutput.WriteLine(descriptor.ToString());

            var id = Guid.NewGuid();
            var clientProvidedCorrelationId = Guid.NewGuid().ToString("N");

            await mediator.CommitAsync(new CreateAccount(id, "Michael Mortensen", "root@gimlichael.dev")
                .SetCorrelationId(clientProvidedCorrelationId)
                .SetCausationId(clientProvidedCorrelationId));

            Assert.Equal(id, deStore.QueryFor<AccountInitiated>().Single().PlatformProviderId);
            Assert.InRange(ieStore.QueryFor<AccountCreated>().Single().Id, 1, 100000);
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreatePlatformProviderAsyncLambda_OnInProcPlatformProviderInitiated_OnOutProcPlatformProviderCreated()
        {
            await using var test = HostTestFactory.Create(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddEfCoreRepository<Account, long, Account>();
                services.AddEfCoreDataStore<PlatformProvider, PlatformProvider>();
                services.AddDataSource<CustomEfCoreDataSource>()
                    .AddUnitOfWork<CustomEfCoreDataSource>();
                services.AddEfCoreAggregateDataSource<PlatformProvider>(o =>
                {
                    o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                    o.ModelConstructor = mb => mb.AddPlatformProvider();
                });
                services.AddSavvyIO(o =>
                {
                    o.AddMediator<Mediator>().EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true);
                });
                services.AddHandlerServicesDescriptor();
                services.AddSingleton<ITestStore<IDomainEvent>, InMemoryTestStore<IDomainEvent>>();
                services.AddSingleton<ITestStore<IIntegrationEvent>, InMemoryTestStore<IIntegrationEvent>>();
            });
            var scope = test.Host.Services.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var descriptor = scope.ServiceProvider.GetRequiredService<IHandlerServicesDescriptor>();
            var deStore = scope.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
            var ieStore = scope.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

            TestOutput.WriteLine(descriptor.ToString());

            var createPlatformProvider = new CreatePlatformProvider("Whitelabel Inc.", "wl", "An example of a whitelabel platform provider.");
            var clientProvidedCorrelationId = Guid.NewGuid().ToString("N");

            await mediator.CommitAsync(createPlatformProvider
                .SetCorrelationId(clientProvidedCorrelationId)
                .SetCausationId(clientProvidedCorrelationId));

            Assert.NotEqual(Guid.Empty, deStore.QueryFor<PlatformProviderInitiated>().Single().Id);
            Assert.Equal(deStore.QueryFor<PlatformProviderInitiated>().Single().Id, ieStore.QueryFor<PlatformProviderCreated>().Single().Id);
        }

        [Fact]
        public async Task QueryTest()
        {
            await using var test = HostTestFactory.Create(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddSingleton<IPersistentRepository<Account, long>, EfCoreRepository<Account, long, Account>>();
                services.AddSingleton<IPersistentRepository<PlatformProvider, Guid>, EfCoreRepository<PlatformProvider, Guid, PlatformProvider>>();
                services.AddEfCoreDataSource<Account>(o => o.ModelConstructor = builder => builder.AddAccount());
                services.AddEfCoreDataSource<PlatformProvider>(o => o.ModelConstructor = builder => builder.AddPlatformProvider());
                services.AddSavvyIO(o => o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery(true).UseAutomaticHandlerDiscovery(true));
                services.AddHandlerServicesDescriptor();
                services.AddSingleton<ITestStore<IDomainEvent>, InMemoryTestStore<IDomainEvent>>();
                services.AddSingleton<ITestStore<IIntegrationEvent>, InMemoryTestStore<IIntegrationEvent>>();
            });
            using var scope = test.Host.Services.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IQueryDispatcher>();
            var descriptor = scope.ServiceProvider.GetRequiredService<IHandlerServicesDescriptor>();
            var deStore = scope.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
            var ieStore = scope.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

            TestOutput.WriteLine(descriptor.ToString());

            var result = await mediator.QueryAsync(new GetFakeAccount(10));

            TestOutput.WriteLine(JsonConvert.SerializeObject(result));

            Assert.Equal(10, result.Id);
        }
    }
}
