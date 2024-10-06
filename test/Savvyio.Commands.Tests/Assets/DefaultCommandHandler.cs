using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Savvyio.Handlers;

namespace Savvyio.Commands.Assets
{
    internal class DefaultCommandHandler : CommandHandler
    {
        private readonly ITestStore<string> _ts;

        public DefaultCommandHandler(ITestStore<string> ts)
        {
            _ts = ts;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<ICommand> handlers)
        {
            handlers.Register<DefaultCommand>(_ => _ts.Add(nameof(DefaultCommand)));
            handlers.RegisterAsync<DefaultCommand>(_ =>
            {
                _ts.Add(nameof(DefaultCommand) + "Async");
                return Task.CompletedTask;
            });
        }
    }
}
