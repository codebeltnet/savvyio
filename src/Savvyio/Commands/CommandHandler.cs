namespace Savvyio.Commands
{
    /// <summary>
    /// Provides a generic and consistent way of handling Command objects that implements the <see cref="ICommand"/> interface. This is an abstract class.
    /// </summary>
    /// <seealso cref="ICommandHandler" />
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

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="ICommand"/> interface.
        /// </summary>
        /// <param name="handler">The registry that store the delegates of type <see cref="ICommand"/>.</param>
        protected abstract void RegisterCommandHandlers(IHandlerRegistry<ICommand> handler);

        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="ICommand" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="ICommand" />.</value>
        public IHandlerActivator<ICommand> Commands => _handlerManager;
    }
}
