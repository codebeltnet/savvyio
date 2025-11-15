using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands.Assets;
using Savvyio.Dispatchers;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Handlers;
using Xunit;

namespace Savvyio.Commands
{
    public class CommandDispatcherTest : Test
    {
        public CommandDispatcherTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CommandDispatcher_Ensure_Initialization_Defaults()
        {
            var sp = new ServiceCollection().AddServiceLocator().BuildServiceProvider();
            var sut = new CommandDispatcher(sp.GetRequiredService<IServiceLocator>());

            Assert.Throws<OrphanedHandlerException>(() => sut.Commit(new DefaultCommand()));
            Assert.ThrowsAsync<OrphanedHandlerException>(() => sut.CommitAsync(new DefaultCommand()));
            Assert.IsAssignableFrom<FireForgetDispatcher>(sut);
            Assert.IsAssignableFrom<ICommandDispatcher>(sut);
            Assert.IsAssignableFrom<Dispatcher>(sut);
            Assert.IsAssignableFrom<IDispatcher>(sut);
        }

        [Fact]
        public async Task CommandDispatcher_ShouldCommit_DefaultCommand()
        {
            var sp = new ServiceCollection().AddServiceLocator().AddTransient<ICommandHandler, DefaultCommandHandler>().AddSingleton<ITestStore<string>, InMemoryTestStore<string>>().BuildServiceProvider();
            var sut = new CommandDispatcher(sp.GetRequiredService<IServiceLocator>());
            var ts = sp.GetRequiredService<ITestStore<string>>();

            sut.Commit(new DefaultCommand());
            await sut.CommitAsync(new DefaultCommand());

            Assert.Collection(ts.Query(),
                s => Assert.Equal(s, nameof(DefaultCommand)),
                s => Assert.Equal(s, nameof(DefaultCommand) + "Async"));
        }
    }
}
