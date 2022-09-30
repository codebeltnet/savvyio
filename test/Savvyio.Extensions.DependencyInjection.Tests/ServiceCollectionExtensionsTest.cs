using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Dispatchers;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Assets;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeHandlerServicesDescriptor()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.EnableHandlerServicesDescriptor());
            var sut2 = sut1.BuildServiceProvider();
            var sut3 = sut2.GetRequiredService<HandlerServicesDescriptor>();
            Assert.NotNull(sut3);
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeServiceFactory()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO();
            var sut2 = sut1.BuildServiceProvider();
            var sut3 = sut2.GetRequiredService<IServiceLocator>();
            Assert.NotNull(sut3);
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeCommandHandlerFromLambda()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(Command).Assembly, typeof(TestCommand).Assembly).EnableHandlerServicesDescriptor().EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            TestOutput.WriteLine(sut2.GetRequiredService<HandlerServicesDescriptor>().ToString());

            Assert.IsType<TestCommandHandler>(sut2.GetRequiredService<ICommandHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeCommandDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyToScan(typeof(Command).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<CommandDispatcher>(sut2.GetRequiredService<ICommandDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeCommandDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyToScan(typeof(Command).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<ICommandDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeCommandHandlerFromLamda()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyRangeToScan(typeof(Command).Assembly, typeof(TestCommand).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<ICommandHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeDomainEventHandlerFromDelegate()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(DomainEvent).Assembly, typeof(TestDomainEvent).Assembly).EnableHandlerServicesDescriptor().EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            TestOutput.WriteLine(sut2.GetRequiredService<HandlerServicesDescriptor>().ToString());

            Assert.IsType<TestDomainEventHandler>(sut2.GetRequiredService<IDomainEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeDomainEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyToScan(typeof(DomainEvent).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DomainEventDispatcher>(sut2.GetRequiredService<IDomainEventDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeDomainEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyToScan(typeof(DomainEvent).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IDomainEventDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeDomainEventHandlerFromDelegate()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyRangeToScan(typeof(DomainEvent).Assembly, typeof(TestDomainEvent).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IDomainEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeIntegrationEventHandlerFromDeclaredMethod()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(IntegrationEvent).Assembly, typeof(TestIntegrationEvent).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<TestIntegrationEventHandler>(sut2.GetRequiredService<IIntegrationEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeIntegrationEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyToScan(typeof(IntegrationEvent).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<IntegrationEventDispatcher>(sut2.GetRequiredService<IIntegrationEventDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeIntegrationEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyToScan(typeof(IntegrationEvent).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IIntegrationEventDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeIntegrationEventHandlerFromDeclaredMethod()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyRangeToScan(typeof(IntegrationEvent).Assembly, typeof(TestIntegrationEvent).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IIntegrationEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeQueryHandlerFromDeclaredMethod()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyRangeToScan(typeof(Query<>).Assembly, typeof(TestQuery).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<TestQueryHandler>(sut2.GetRequiredService<IQueryHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeQueryDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AddAssemblyToScan(typeof(Query<>).Assembly).EnableDispatcherDiscovery().EnableHandlerDiscovery());
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<QueryDispatcher>(sut2.GetRequiredService<IQueryDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeQueryDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyToScan(typeof(Query<>).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IQueryDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeQueryHandlerFromDeclaredMethod()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddAssemblyRangeToScan(typeof(Query<>).Assembly, typeof(TestQuery).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IQueryHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldHaveAllManuallyAddedHandlersAndDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AddCommandDispatcher()
                    .AddDomainEventDispatcher()
                    .AddIntegrationEventDispatcher()
                    .AddQueryDispatcher();
                o.AddCommandHandler<TestCommandHandler>()
                    .AddDomainEventHandler<TestDomainEventHandler>()
                    .AddIntegrationEventHandler<TestIntegrationEventHandler>()
                    .AddQueryHandler<TestQueryHandler>();
            });
            var sut2 = sut1.BuildServiceProvider();
            
            Assert.NotNull(sut2.GetService<ICommandDispatcher>());
            Assert.NotNull(sut2.GetService<IDomainEventDispatcher>());
            Assert.NotNull(sut2.GetService<IIntegrationEventDispatcher>());
            Assert.NotNull(sut2.GetService<IQueryDispatcher>());

            Assert.NotNull(sut2.GetService<ICommandHandler>());
            Assert.NotNull(sut2.GetService<IDomainEventHandler>());
            Assert.NotNull(sut2.GetService<IIntegrationEventHandler>());
            Assert.NotNull(sut2.GetService<IQueryHandler>());
        }
    }
}
