namespace Savvyio.Events
{
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

        protected abstract void RegisterEventHandlers(IHandlerRegistry<IIntegrationEvent> handler);

        public IHandlerActivator<IIntegrationEvent> IntegrationEvents => _handlerManager;
    }
}
