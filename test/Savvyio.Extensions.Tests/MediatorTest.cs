using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
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
using Savvyio.Assets.Storage;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions.Dispatchers;
using Savvyio.Extensions.Storage;
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
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>().IncludeHandlerServicesDescriptor = true)))
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
                services.AddEfCoreRepository<EfCoreRepository<Account, long, Account>, Account, long>();
                services.AddEfCoreDataAccessObject<EfCoreDataAccessObject<PlatformProvider, PlatformProvider>, PlatformProvider>();
                //services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(1, 101));
                //services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                services.AddEfCoreDataStore<ActiveRecordEfCoreDataStore<Account>>();
                services.AddEfCoreDataStore<PlatformProvider>(o =>
                {
                    o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                    o.ModelConstructor = mb => mb.AddPlatformProvider();
                });
                //services.AddActiveRecordRepository<Account, long>();
                //services.AddActiveRecordRepository<PlatformProvider, Guid>();
                services.AddSavvyIO(registry => registry.AddMediator<Mediator>().IncludeHandlerServicesDescriptor = true);
                services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
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
                services.AddEfCoreRepository<EfCoreRepository<Account, long, Account>, Account, long>();
                services.AddEfCoreDataAccessObject<EfCoreDataAccessObject<PlatformProvider, PlatformProvider>, PlatformProvider>();
                services.AddEfCoreDataStore<ActiveRecordEfCoreDataStore<Account>>();
                services.AddEfCoreDataStore<PlatformProvider>(o =>
                {
                    o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                    o.ModelConstructor = mb => mb.AddPlatformProvider();
                });
                //services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(1, 101));
                //services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                //services.AddActiveRecordRepository<Account, long>();
                //services.AddActiveRecordRepository<PlatformProvider, Guid>();
                services.AddSavvyIO(registry =>
                {
                    registry.AddMediator<Mediator>();
                    registry.IncludeHandlerServicesDescriptor = true;
                });
                services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
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
                       services.AddEfCoreDataStore<ActiveRecordEfCoreDataStore<Account>>();
                       services.AddEfCoreDataStore<ActiveRecordEfCoreDataStore<PlatformProvider>>();
                       //services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(1, 101));
                       //services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                       //services.AddActiveRecordRepository<Account, long>();
                       //services.AddActiveRecordRepository<PlatformProvider, Guid>();
                       services.AddSavvyIO(options =>
                       {
                           //options.AddQueryHandler<AccountQueryHandler>();
                           //options.AddService<IQueryHandler>();
                           options.IncludeHandlerServicesDescriptor = true;
                           //options.AddDispatcher<IQueryDispatcher, QueryDispatcher>();
                       });
                       services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                       services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
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


    //public class MediatorTest : HostTest<HostFixture>
    //{
    //    private readonly HostFixture _hostFixture;

    //    public MediatorTest(HostFixture hostFixture, ITestOutputHelper output = null, Type callerType = null) : base(hostFixture, output, callerType)
    //    {
    //        _hostFixture = hostFixture;
    //    }

    //    [Fact]
    //    public void HostFixture_MediatorShouldBeRegistered()
    //    {
    //        _hostFixture.ServiceProvider.GetRequiredService<IMediator>();
    //    }

    //    public override void ConfigureServices(IServiceCollection services)
    //    {
    //        services.AddMediator(registry => registry.AddHandlersFromCurrentDomain());
    //    }
    //}
}
