namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a generic and consistent way of handling Domain Event (as specified in Domain Driven Design) objects that implements the <see cref="IDomainEvent"/> interface. This is an abstract class.
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

        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IDomainEvent" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IDomainEvent" />.</value>
        public IHandlerActivator<IDomainEvent> DomainEvents => _handlerManager;
    }
}
