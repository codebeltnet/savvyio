namespace Savvyio.Events
{
    /// <summary>
    /// Provides a generic and consistent way of handling Integration Event objects that implements the <see cref="IIntegrationEvent"/> interface. This is an abstract class.
    /// </summary>
    /// <seealso cref="IIntegrationEventHandler" />
    public abstract class IntegrationEventHandler : IIntegrationEventHandler
    {
        private readonly HandlerManager<IIntegrationEvent> _handlerManager = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventHandler"/> class.
        /// </summary>
        protected IntegrationEventHandler()
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterEventHandlers(_handlerManager);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="IIntegrationEvent"/> interface.
        /// </summary>
        /// <param name="handler">The registry that store the delegates of type <see cref="IIntegrationEvent"/>.</param>
        protected abstract void RegisterEventHandlers(IHandlerRegistry<IIntegrationEvent> handler);

        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IIntegrationEvent" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IIntegrationEvent" />.</value>
        public IHandlerActivator<IIntegrationEvent> IntegrationEvents => _handlerManager;
    }
}
