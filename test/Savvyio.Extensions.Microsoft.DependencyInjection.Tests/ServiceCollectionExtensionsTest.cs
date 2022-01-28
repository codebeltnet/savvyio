using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions.Assets;
using Savvyio.Extensions.Microsoft.DependencyInjection;
using Savvyio.Queries;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions
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
            sut1.AddSavvyIO(o => o.IncludeHandlerServicesDescriptor = true);
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
            var sut3 = sut2.GetRequiredService<Func<Type, IEnumerable<object>>>();
            Assert.NotNull(sut3);
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeCommandHandlerFromLamda()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Command).Assembly, typeof(TestCommand).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<TestCommandHandler>(sut2.GetRequiredService<ICommandHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeCommandDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Command).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<CommandDispatcher>(sut2.GetRequiredService<ICommandDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeCommandDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AutoResolveDispatchers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Command).Assembly);
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
                o.AutoResolveHandlers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Command).Assembly, typeof(TestCommand).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<ICommandHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeDomainEventHandlerFromDelegate()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEvent).Assembly, typeof(TestDomainEvent).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<TestDomainEventHandler>(sut2.GetRequiredService<IDomainEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeDomainEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEvent).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DomainEventDispatcher>(sut2.GetRequiredService<IDomainEventDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeDomainEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AutoResolveDispatchers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEvent).Assembly);
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
                o.AutoResolveHandlers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(DomainEvent).Assembly, typeof(TestDomainEvent).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IDomainEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeIntegrationEventHandlerFromDeclaredMethod()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(IntegrationEvent).Assembly, typeof(TestIntegrationEvent).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<TestIntegrationEventHandler>(sut2.GetRequiredService<IIntegrationEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeIntegrationEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(IntegrationEvent).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<IntegrationEventDispatcher>(sut2.GetRequiredService<IIntegrationEventDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeIntegrationEventDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AutoResolveDispatchers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(IntegrationEvent).Assembly);
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
                o.AutoResolveHandlers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(IntegrationEvent).Assembly, typeof(TestIntegrationEvent).Assembly);
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.Null(sut2.GetService<IIntegrationEventHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeQueryHandlerFromDeclaredMethod()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Query<>).Assembly, typeof(TestQuery).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<TestQueryHandler>(sut2.GetRequiredService<IQueryHandler>());
        }

        [Fact]
        public void AddSavvyIO_ShouldIncludeQueryDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o => o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Query<>).Assembly));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<QueryDispatcher>(sut2.GetRequiredService<IQueryDispatcher>());
        }

        [Fact]
        public void AddSavvyIO_ShouldNotIncludeQueryDispatchers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddSavvyIO(o =>
            {
                o.AutoResolveDispatchers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Query<>).Assembly);
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
                o.AutoResolveHandlers = false;
                o.AssembliesToScan = Arguments.ToEnumerableOf(typeof(Query<>).Assembly, typeof(TestQuery).Assembly);
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
                o.AutoResolveHandlers = false;
                o.AutoResolveDispatchers = false;
                o.AddDispatcher<ICommandDispatcher, CommandDispatcher>()
                    .AddDispatcher<IDomainEventDispatcher, DomainEventDispatcher>()
                    .AddDispatcher<IIntegrationEventDispatcher, IntegrationEventDispatcher>()
                    .AddDispatcher<IQueryDispatcher, QueryDispatcher>();
                o.AddHandler<ICommandHandler, ICommand, TestCommandHandler>()
                    .AddHandler<IDomainEventHandler, IDomainEvent, TestDomainEventHandler>()
                    .AddHandler<IIntegrationEventHandler, IIntegrationEvent, TestIntegrationEventHandler>()
                    .AddHandler<IQueryHandler, IQuery, TestQueryHandler>();
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
