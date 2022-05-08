using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Commands.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands
{
    public class CommandHandlerTest: Test
    {
        public CommandHandlerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CommandHandler_Ensure_Initialization_Defaults()
        {
            var sut = new DefaultCommandHandler(new InMemoryTestStore<string>());

            Assert.IsAssignableFrom<CommandHandler>(sut);
            Assert.IsAssignableFrom<ICommandHandler>(sut);
            Assert.True(sut.Delegates.TryInvoke(new DefaultCommand()));
            Assert.True((await sut.Delegates.TryInvokeAsync(new DefaultCommand())).Succeeded);
            Assert.False(sut.Delegates.TryInvoke(new CreatePlatformProvider("", "", "")));
            Assert.False((await sut.Delegates.TryInvokeAsync(new CreatePlatformProvider("", "", ""))).Succeeded);
        }
    }
}
