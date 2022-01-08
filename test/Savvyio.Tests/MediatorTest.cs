using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Events;
using Savvyio.Assets.Queries;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions;
using Savvyio.Extensions.Domain;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio
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
                Assert.Throws<InvalidOperationException>(() => host.ServiceProvider.GetRequiredService<SavvyioServiceDescriptor>());
            }
        }

        [Fact]
        public void Host_MediatorDescriptorShouldBeRegistered()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddSavvyIO(registry => registry.AddMediator<Mediator>().IncludeServicesDescriptor = true)))
            {
                var descriptor = host.ServiceProvider.GetRequiredService<SavvyioServiceDescriptor>();

                Assert.IsType<SavvyioServiceDescriptor>(descriptor);

                TestOutput.WriteLine(descriptor.ToString());
            }
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreateAccountAsync_OnInProcAccountCreated_OnOutProcAccountCreated()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(1, 101));
                services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                services.AddActiveRecordRepository<Account, long>();
                services.AddActiveRecordRepository<PlatformProvider, Guid>();
                services.AddSavvyIO(registry => registry.AddMediator<Mediator>().IncludeServicesDescriptor = true);
                services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
            }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();
                var descriptor = host.ServiceProvider.GetRequiredService<SavvyioServiceDescriptor>();
                var deStore = host.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
                var ieStore = host.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

                TestOutput.WriteLine(descriptor.ToString());

                var id = Guid.NewGuid();
                var clientProvidedCorrelationId = Guid.NewGuid().ToString("N");

                await mediator.CommitAsync(new CreateAccount(id, "Michael Mortensen", "root@gimlichael.dev")
                    .SetCorrelationId(clientProvidedCorrelationId)
                    .SetCausationId(clientProvidedCorrelationId));

                Assert.Equal(id, deStore.QueryFor<AccountInitiated>().Single().PlatformProviderId);
                Assert.InRange(ieStore.QueryFor<AccountCreated>().Single().Id, 1, 100);
            }
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreatePlatformProviderAsyncLambda_OnInProcPlatformProviderInitiated_OnOutProcPlatformProviderCreated()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(1, 101));
                services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                services.AddActiveRecordRepository<Account, long>();
                services.AddActiveRecordRepository<PlatformProvider, Guid>();
                services.AddSavvyIO(registry =>
                {
                    registry.AutoResolveDispatchers = false;
                    registry.AddMediator<Mediator>();
                    registry.IncludeServicesDescriptor = true;
                });
                services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
            }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();
                var descriptor = host.ServiceProvider.GetRequiredService<SavvyioServiceDescriptor>();
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
                       services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(1, 101));
                       services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                       services.AddActiveRecordRepository<Account, long>();
                       services.AddActiveRecordRepository<PlatformProvider, Guid>();
                       services.AddSavvyIO(options =>
                       {
                           //options.AddQueryHandler<AccountQueryHandler>();
                           //options.AddService<IQueryHandler>();
                           options.IncludeServicesDescriptor = true;
                           //options.AddDispatcher<IQueryDispatcher, QueryDispatcher>();
                       });
                       services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                       services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
                   }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IQueryDispatcher>();
                var descriptor = host.ServiceProvider.GetRequiredService<SavvyioServiceDescriptor>();
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
