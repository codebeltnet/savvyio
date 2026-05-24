using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Dispatchers;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;
using Xunit;

namespace Savvyio.EventDriven
{
    public class IntegrationEventDispatcherTest : Test
    {
        public IntegrationEventDispatcherTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Publish_ShouldDispatchIntegrationEvent()
        {
            var services = new ServiceCollection()
                .AddServiceLocator()
                .AddSingleton<ITestStore<string>, InMemoryTestStore<string>>()
                .AddTransient<IIntegrationEventHandler, TestIntegrationEventHandler>();
            var provider = services.BuildServiceProvider();
            var sut = new IntegrationEventDispatcher(provider.GetRequiredService<IServiceLocator>());

            sut.Publish(new TestIntegrationEvent("published"));

            Assert.Collection(provider.GetRequiredService<ITestStore<string>>().Query(), item => Assert.Equal("published", item));
        }

        private sealed record TestIntegrationEvent(string Value) : IntegrationEvent;

        private sealed class TestIntegrationEventHandler : IntegrationEventHandler
        {
            private readonly ITestStore<string> _store;

            public TestIntegrationEventHandler(ITestStore<string> store)
            {
                _store = store;
            }

            protected override void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers)
            {
                handlers.Register<TestIntegrationEvent>(message => _store.Add(message.Value));
            }
        }
    }
}
