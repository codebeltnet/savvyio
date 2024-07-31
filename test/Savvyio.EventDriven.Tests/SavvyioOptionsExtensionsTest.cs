using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.EventDriven
{
    public class SavvyioOptionsExtensionsTest : Test
    {
        public SavvyioOptionsExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddHandlers_ShouldNotAddAnyHandlersFromCurrentDomain()
        {
            var sut = new SavvyioOptions().AddHandlers();
            Assert.Empty(sut.HandlerServiceTypes);
            Assert.Empty(sut.HandlerImplementationTypes);
        }

        [Fact]
        public void AddIntegrationEventHandler_ShouldAddDomainEventHandler()
        {
            var sut = new SavvyioOptions().AddIntegrationEventHandler<AccountEventHandler>();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes, type => Assert.Equal(typeof(IIntegrationEventHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes, type => Assert.Equal(typeof(AccountEventHandler), type));
        }

        [Fact]
        public void AddIntegrationEventDispatcher_ShouldAddDomainEventDispatcher()
        {
            var sut = new SavvyioOptions().AddIntegrationEventDispatcher();

            Assert.NotEmpty(sut.DispatcherServiceTypes);
            Assert.NotEmpty(sut.DispatcherImplementationTypes);
            Assert.Collection(sut.DispatcherServiceTypes, type => Assert.Equal(typeof(IIntegrationEventDispatcher), type));
            Assert.Collection(sut.DispatcherImplementationTypes, type => Assert.Equal(typeof(IntegrationEventDispatcher), type));
        }
    }
}
