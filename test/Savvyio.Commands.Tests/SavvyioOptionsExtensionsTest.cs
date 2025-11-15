using Codebelt.Extensions.Xunit;
using Savvyio.Assets;
using Xunit;

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
        public void AddCommandHandler_ShouldAddCommandHandler()
        {
            var sut = new SavvyioOptions().AddCommandHandler<AccountCommandHandler>();

            Assert.NotEmpty(sut.HandlerServiceTypes);
            Assert.NotEmpty(sut.HandlerImplementationTypes);
            Assert.Collection(sut.HandlerServiceTypes, type => Assert.Equal(typeof(ICommandHandler), type));
            Assert.Collection(sut.HandlerImplementationTypes, type => Assert.Equal(typeof(AccountCommandHandler), type));
        }

        [Fact]
        public void AddCommandDispatcher_ShouldAddCommandDispatcher()
        {
            var sut = new SavvyioOptions().AddCommandDispatcher();

            Assert.NotEmpty(sut.DispatcherServiceTypes);
            Assert.NotEmpty(sut.DispatcherImplementationTypes);
            Assert.Collection(sut.DispatcherServiceTypes, type => Assert.Equal(typeof(ICommandDispatcher), type));
            Assert.Collection(sut.DispatcherImplementationTypes, type => Assert.Equal(typeof(CommandDispatcher), type));
        }
    }
}
