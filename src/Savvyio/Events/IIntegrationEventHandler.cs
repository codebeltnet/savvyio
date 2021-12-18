namespace Savvyio.Events
{
    /// <summary>
    /// Specifies a handler resposible for objects that implements the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    /// <seealso cref="IHandler{IIntegrationEvent}" />
    public interface IIntegrationEventHandler : IHandler<IIntegrationEvent>
    {
        /// <summary>
        /// Gets the activator responsible of invoking delegates that handles <see cref="IIntegrationEvent"/>.
        /// </summary>
        /// <value>The activator responsible of invoking delegates that handles <see cref="IIntegrationEvent"/>.</value>
        IFireForgetActivator<IIntegrationEvent> IntegrationEvents { get; }
    }
}
