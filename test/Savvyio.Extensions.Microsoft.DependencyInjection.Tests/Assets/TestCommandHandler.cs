using System;
using Savvyio.Commands;
using Savvyio.Handlers;

namespace Savvyio.Extensions.Assets
{
    public class TestCommandHandler : CommandHandler
    {
        public TestCommandHandler()
        {
        }

        protected override void RegisterDelegates(IFireForgetRegistry<ICommand> handlers)
        {
            handlers.Register<TestCommand>(_ => throw new NotImplementedException());
        }
    }
}
