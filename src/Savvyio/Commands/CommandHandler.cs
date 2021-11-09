namespace Savvyio.Commands
{
    public abstract class CommandHandler : ICommandHandler
    {
        private readonly HandlerManager<ICommand> _handlerManager = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        protected CommandHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterCommandHandlers(_handlerManager);
        }

        protected abstract void RegisterCommandHandlers(IHandlerRegistry<ICommand> handler);

        public IHandlerActivator<ICommand> Commands => _handlerManager;
    }
}
