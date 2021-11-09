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
using Savvyio.Domain;
using Savvyio.Events;
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
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddMediator(registry => registry.AddHandlersFromCurrentDomain())))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();

                Assert.IsType<Mediator>(mediator);
            }
        }

        [Fact]
        public void Host_MediatorDescriptorShouldNotBeRegistered_UsingDefaultOptions()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddMediator(registry => registry.AddHandlersFromCurrentDomain())))
            {
                Assert.Throws<InvalidOperationException>(() => host.ServiceProvider.GetRequiredService<MediatorDescriptor>());
            }
        }

        [Fact]
        public void Host_MediatorDescriptorShouldBeRegistered()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services => services.AddMediator(registry => registry.AddHandlersFromCurrentDomain(), o => o.IncludeMediatorDescriptor = true)))
            {
                var descriptor = host.ServiceProvider.GetRequiredService<MediatorDescriptor>();

                Assert.IsType<MediatorDescriptor>(descriptor);

                TestOutput.WriteLine(descriptor.ToString());
            }
        }

        [Fact]
        public async Task Mediator_ShouldInvoke_CreateAccountAsync_OnInProcAccountCreated_OnOutProcAccountCreated()
        {
            using (var host = GenericHostTestFactory.CreateGenericHostTest(services =>
            {
                services.AddSingleton(TestOutput);
                services.AddInMemoryActiveRecordStore<Account, long>(o => o.IdentityProvider = _ => Generate.RandomNumber(101));
                services.AddInMemoryActiveRecordStore<PlatformProvider, Guid>();
                services.AddActiveRecordRepository<Account, long>();
                services.AddActiveRecordRepository<PlatformProvider, Guid>();
                services.AddMediator(registry => registry.AddHandlersFromCurrentDomain(), o => o.IncludeMediatorDescriptor = true);
                services.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
                services.AddScoped<ITestStore<IIntegrationEvent>, IntegrationEventStore>();
            }))
            {
                var mediator = host.ServiceProvider.GetRequiredService<IMediator>();
                var descriptor = host.ServiceProvider.GetRequiredService<MediatorDescriptor>();
                var deStore = host.ServiceProvider.GetRequiredService<ITestStore<IDomainEvent>>();
                var ieStore = host.ServiceProvider.GetRequiredService<ITestStore<IIntegrationEvent>>();

                TestOutput.WriteLine(descriptor.ToString());

                var id = Guid.NewGuid();

                await mediator.CommitAsync(new CreateAccount(id, "Michael Mortensen", "root@gimlichael.dev"));
                
                Assert.Equal(id, deStore.QueryFor<AccountInitiated>().Single().PlatformProviderId);
                Assert.InRange(ieStore.QueryFor<AccountCreated>().Single().Id, 1, 100);
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
