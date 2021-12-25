namespace Savvyio.Commands
{
    /// <summary>
    /// Provides a generic and consistent way of handling Command objects that implements the <see cref="ICommand"/> interface. This is an abstract class.
    /// </summary>
    /// <seealso cref="ICommandHandler" />
    public abstract class CommandHandler : ICommandHandler
    {
        private readonly FireForgetManager<ICommand> _manager = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class.
        /// </summary>
        protected CommandHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterDelegates(_manager);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="ICommand"/> interface.
        /// </summary>
        /// <param name="handlers">The registry that store the delegates of type <see cref="ICommand"/>.</param>
        protected abstract void RegisterDelegates(IFireForgetRegistry<ICommand> handlers);
        
        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="ICommand" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="ICommand" />.</value>
        public IFireForgetActivator<ICommand> Delegates => _manager;
    }
}
