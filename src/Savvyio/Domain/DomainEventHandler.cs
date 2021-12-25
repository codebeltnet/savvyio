namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a generic and consistent way of handling Domain Event (as specified in Domain Driven Design) objects that implements the <see cref="IDomainEvent"/> interface. This is an abstract class.
    /// </summary>
    /// <seealso cref="IDomainEventHandler" />
    public abstract class DomainEventHandler : IDomainEventHandler
    {
        private readonly FireForgetManager<IDomainEvent> _manager = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventHandler"/> class.
        /// </summary>
        protected DomainEventHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterDelegates(_manager);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="IDomainEvent"/> interface.
        /// </summary>
        /// <param name="handlers">The registry that store the delegates of type <see cref="IDomainEvent"/>.</param>
        protected abstract void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers);

        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IDomainEvent" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IDomainEvent" />.</value>
        public IFireForgetActivator<IDomainEvent> Delegates => _manager;
    }
}
