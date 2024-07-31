using Savvyio.Handlers;

namespace Savvyio.EventDriven
{
    /// <summary>
    /// Provides a generic and consistent way of handling Integration Event objects that implements the <see cref="IIntegrationEvent"/> interface. This is an abstract class.
    /// </summary>
    /// <seealso cref="IIntegrationEventHandler" />
    public abstract class IntegrationEventHandler : IIntegrationEventHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventHandler"/> class.
        /// </summary>
        protected IntegrationEventHandler()
        {
            Delegates = HandlerFactory.CreateFireForget<IIntegrationEvent>(RegisterDelegates);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="IIntegrationEvent"/> interface.
        /// </summary>
        /// <param name="handlers">The registry that store the delegates of type <see cref="IIntegrationEvent"/>.</param>
        protected abstract void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers);

        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IIntegrationEvent" />.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IIntegrationEvent" />.</value>
        public IFireForgetActivator<IIntegrationEvent> Delegates { get; }
    }
}
