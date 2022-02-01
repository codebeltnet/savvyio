using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Events;
using Savvyio.Assets.Queries;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Savvyio.Queries;
using Savvyio.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions
{
    public class MediatorTest : Test
    {
        public MediatorTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Host_MediatorShouldBeRegistered_UsingDefaultOptions()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>())))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();

                Assert.IsType<Mediator>(mediator);
            }
        }

        [Fact]
        public void Host_MediatorDescriptorShouldNotBeRegistered_UsingDefaultOptions()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>())))
            {
                Assert.Throws<InvalidOperationException>(() => host.ServiceProvider.GetRequiredService<HandlerServicesDescriptor>());
            }
        }

        [Fact]
        public void Host_MediatorDescriptorShouldBeRegistered()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>().EnableHandlerServicesDescriptor())))
            {
                var descriptor = host.ServiceProvider.GetRequiredService<HandlerServicesDescriptor>();

                Assert.IsType<HandlerServicesDescriptor>(descriptor);

                TestOutput.WriteLine(descriptor.ToString());
            }
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreateAccountAsync_OnInProcAccountCreated_OnOutProcAccountCreated()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddEfCoreRepository<Account, long, DbMarker>();
                services.AddEfCoreDataAccessObject<PlatformProvider, PlatformProvider>();
                services.AddEfCoreDataStore<CustomEfCoreDataStore>();
                services.AddEfCoreDataStore<PlatformProvider>(o =>
                {
                    o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                    o.ModelConstructor = mb => mb.AddPlatformProvider();
                });
                services.AddSavvyIO(o => o.EnableAutomaticDispatcherDiscovery().EnableAutomaticHandlerDiscovery().EnableHandlerServicesDescriptor().AddMediator<Mediator>());
                services.AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();
                services.AddScoped<ITestStore<IIntegrationEvent>, InMemUnitTestStore<IIntegrationEvent>>();
            }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();
                var descriptor = host.ServiceProvider.GetRequiredService<HandlerServicesDescriptor>();
                var deStore = host.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
                var ieStore = host.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

                TestOutput.WriteLine(descriptor.ToString());

                var id = Guid.NewGuid();
                var clientProvidedCorrelationId = Guid.NewGuid().ToString("N");

                await mediator.CommitAsync(new CreateAccount(id, "Michael Mortensen", "root@gimlichael.dev")
                    .SetCorrelationId(clientProvidedCorrelationId)
                    .SetCausationId(clientProvidedCorrelationId));

                Assert.Equal(id, deStore.QueryFor<AccountInitiated>().Single().PlatformProviderId);
                Assert.InRange(ieStore.QueryFor<AccountCreated>().Single().Id, 1, 100000);
            }
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreatePlatformProviderAsyncLambda_OnInProcPlatformProviderInitiated_OnOutProcPlatformProviderCreated()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddEfCoreRepository<Account, long>();
                services.AddEfCoreDataAccessObject<PlatformProvider, PlatformProvider>();
                services.AddEfCoreDataStore<CustomEfCoreDataStore>();
                services.AddEfCoreAggregateDataStore<PlatformProvider>(o =>
                {
                    o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                    o.ModelConstructor = mb => mb.AddPlatformProvider();
                });
                services.AddSavvyIO(o =>
                {
                    o.AddMediator<Mediator>().EnableHandlerServicesDescriptor().EnableAutomaticDispatcherDiscovery().EnableAutomaticHandlerDiscovery();
                });
                services.AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();
                services.AddScoped<ITestStore<IIntegrationEvent>, InMemUnitTestStore<IIntegrationEvent>>();
            }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();
                var descriptor = host.ServiceProvider.GetRequiredService<HandlerServicesDescriptor>();
                var deStore = host.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
                var ieStore = host.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

                TestOutput.WriteLine(descriptor.ToString());

                var createPlatformProvider = new CreatePlatformProvider("Whitelabel Inc.", "wl", "An example of a whitelabel platform provider.");
                var clientProvidedCorrelationId = Guid.NewGuid().ToString("N");

                await mediator.CommitAsync(createPlatformProvider
                    .SetCorrelationId(clientProvidedCorrelationId)
                    .SetCausationId(clientProvidedCorrelationId));

                Assert.NotEqual(Guid.Empty, deStore.QueryFor<PlatformProviderInitiated>().Single().Id);
                Assert.Equal(deStore.QueryFor<PlatformProviderInitiated>().Single().Id, ieStore.QueryFor<PlatformProviderCreated>().Single().Id);
            }
        }

        [Fact]
        public async Task QueryTest()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services =>
                   {
                       services.AddSingleton(TestOutput);
                       services.AddScoped<IPersistentRepository<Account, long>, EfCoreRepository<Account, long, Account>>();
                       services.AddScoped<IPersistentRepository<PlatformProvider, Guid>, EfCoreRepository<PlatformProvider, Guid, PlatformProvider>>();
                       services.AddEfCoreDataStore<Account>(o => o.ModelConstructor = builder => builder.AddAccount());
                       services.AddEfCoreDataStore<PlatformProvider>(o => o.ModelConstructor = builder => builder.AddPlatformProvider());
                       services.AddSavvyIO(o => o.EnableHandlerServicesDescriptor().EnableAutomaticDispatcherDiscovery().EnableAutomaticHandlerDiscovery());
                       services.AddScoped<ITestStore<IDomainEvent>, InMemUnitTestStore<IDomainEvent>>();
                       services.AddScoped<ITestStore<IIntegrationEvent>, InMemUnitTestStore<IIntegrationEvent>>();
                   }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IQueryDispatcher>();
                var descriptor = host.ServiceProvider.GetRequiredService<HandlerServicesDescriptor>();
                var deStore = host.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
                var ieStore = host.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

                TestOutput.WriteLine(descriptor.ToString());

                var id = Guid.NewGuid();
                var clientProvidedCorrelationId = Guid.NewGuid().ToString("N");

                var result = await mediator.QueryAsync(new GetAccount(10));

                TestOutput.WriteLine(result);
            }
        }
    }
}
