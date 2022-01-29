using Cuemon.Extensions.Xunit;
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
        public void AddHandlers_ShouldAddDomainEventHandler()
        {
            var sut = new SavvyioOptions().AddDomainEventHandler<AccountDomainEventHandler>();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes, type => Assert.Equal(typeof(IDomainEventHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes, type => Assert.Equal(typeof(AccountDomainEventHandler), type));
        }
    }
}
