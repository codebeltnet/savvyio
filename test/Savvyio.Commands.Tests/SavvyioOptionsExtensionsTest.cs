using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands
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
            var sut = new SavvyioOptions().AddCommandHandler<AccountCommandHandler>();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes, type => Assert.Equal(typeof(ICommandHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes, type => Assert.Equal(typeof(AccountCommandHandler), type));
        }
    }
}
