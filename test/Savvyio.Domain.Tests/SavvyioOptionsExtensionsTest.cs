using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Domain.Handlers;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain
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
        public void AddDomainEventHandler_ShouldAddDomainEventHandler()
        {
            var sut = new SavvyioOptions().AddDomainEventHandler<AccountDomainEventHandler>();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes, type => Assert.Equal(typeof(IDomainEventHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes, type => Assert.Equal(typeof(AccountDomainEventHandler), type));
        }

        [Fact]
        public void AddDomainEventDispatcher_ShouldAddDomainEventDispatcher()
        {
            var sut = new SavvyioOptions().AddDomainEventDispatcher();

            Assert.NotEmpty(sut.DispatcherServiceTypes);
            Assert.NotEmpty(sut.DispatcherImplementationTypes);
            Assert.Collection(sut.DispatcherServiceTypes, type => Assert.Equal(typeof(IDomainEventDispatcher), type));
            Assert.Collection(sut.DispatcherImplementationTypes, type => Assert.Equal(typeof(DomainEventDispatcher), type));
        }
    }
}
