using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.Dispatchers;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;
using Savvyio.Queries;
using Xunit;

namespace Savvyio.Extensions
{
    public class MediatorSyncTest : Test
    {
        public MediatorSyncTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Commit_ShouldDispatchCommandSynchronously()
        {
            var provider = CreateServiceProvider(services => services.AddTransient<ICommandHandler, TestMediatorCommandHandler>());
            var sut = new Mediator(provider.GetRequiredService<IServiceLocator>());

            sut.Commit(new TestMediatorCommand());

            Assert.Collection(provider.GetRequiredService<ITestStore<string>>().Query(), item => Assert.Equal("command", item));
        }

        [Fact]
        public void Raise_ShouldDispatchDomainEventSynchronously()
        {
            var provider = CreateServiceProvider(services => services.AddTransient<IDomainEventHandler, TestMediatorDomainEventHandler>());
            var sut = new Mediator(provider.GetRequiredService<IServiceLocator>());

            sut.Raise(new TestMediatorDomainEvent());

            Assert.Collection(provider.GetRequiredService<ITestStore<string>>().Query(), item => Assert.Equal("domain-event", item));
        }

        [Fact]
        public void Publish_ShouldDispatchIntegrationEventSynchronously()
        {
            var provider = CreateServiceProvider(services => services.AddTransient<IIntegrationEventHandler, TestMediatorIntegrationEventHandler>());
            var sut = new Mediator(provider.GetRequiredService<IServiceLocator>());

            sut.Publish(new TestMediatorIntegrationEvent());

            Assert.Collection(provider.GetRequiredService<ITestStore<string>>().Query(), item => Assert.Equal("integration-event", item));
        }

        [Fact]
        public void Query_ShouldDispatchQuerySynchronously()
        {
            var provider = CreateServiceProvider(services => services.AddTransient<IQueryHandler, TestMediatorQueryHandler>());
            var sut = new Mediator(provider.GetRequiredService<IServiceLocator>());

            var result = sut.Query(new TestMediatorQuery());

            Assert.Equal("query", result);
        }

        private static ServiceProvider CreateServiceProvider(System.Action<IServiceCollection> setup)
        {
            var services = new ServiceCollection()
                .AddServiceLocator()
                .AddSingleton<ITestStore<string>, InMemoryTestStore<string>>();

            setup(services);
            return services.BuildServiceProvider();
        }

        private sealed record TestMediatorCommand : Command;

        private sealed record TestMediatorDomainEvent : DomainEvent;

        private sealed record TestMediatorIntegrationEvent : IntegrationEvent;

        private sealed record TestMediatorQuery : Query<string>;

        private sealed class TestMediatorCommandHandler : CommandHandler
        {
            private readonly ITestStore<string> _store;

            public TestMediatorCommandHandler(ITestStore<string> store)
            {
                _store = store;
            }

            protected override void RegisterDelegates(IFireForgetRegistry<ICommand> handlers)
            {
                handlers.Register<TestMediatorCommand>(_ => _store.Add("command"));
            }
        }

        private sealed class TestMediatorDomainEventHandler : DomainEventHandler
        {
            private readonly ITestStore<string> _store;

            public TestMediatorDomainEventHandler(ITestStore<string> store)
            {
                _store = store;
            }

            protected override void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers)
            {
                handlers.Register<TestMediatorDomainEvent>(_ => _store.Add("domain-event"));
            }
        }

        private sealed class TestMediatorIntegrationEventHandler : IntegrationEventHandler
        {
            private readonly ITestStore<string> _store;

            public TestMediatorIntegrationEventHandler(ITestStore<string> store)
            {
                _store = store;
            }

            protected override void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers)
            {
                handlers.Register<TestMediatorIntegrationEvent>(_ => _store.Add("integration-event"));
            }
        }

        private sealed class TestMediatorQueryHandler : QueryHandler
        {
            protected override void RegisterDelegates(IRequestReplyRegistry<IQuery> handlers)
            {
                handlers.Register<TestMediatorQuery, string>(_ => "query");
            }
        }
    }
}
