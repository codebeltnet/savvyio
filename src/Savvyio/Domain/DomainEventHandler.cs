namespace Savvyio.Domain
{
    /// <summary>
    /// Class DomainEventHandler.
    /// </summary>
    /// <seealso cref="IDomainEventHandler" />
    public abstract class DomainEventHandler : IDomainEventHandler
    {
        private readonly HandlerManager<IDomainEvent> _handlerManager = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventHandler"/> class.
        /// </summary>
        protected DomainEventHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterDomainEventHandlers(_handlerManager);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="IDomainEvent"/> interface.
        /// </summary>
        /// <param name="handler">The registry that store the delegates of type <see cref="IDomainEvent"/>.</param>
        protected abstract void RegisterDomainEventHandlers(IHandlerRegistry<IDomainEvent> handler);

        public IHandlerActivator<IDomainEvent> DomainEvents => _handlerManager;
    }
}
