namespace Savvyio.EventDriven.Messaging
{
    /// <summary>
    /// Specifies an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to be made aware of that something has happened.
    /// </summary>
    /// <seealso cref="IIntegrationEventPublisher" />
    /// <seealso cref="IIntegrationEventSubscriber" />
    public interface IIntegrationEventBus : IIntegrationEventPublisher, IIntegrationEventSubscriber
    {
    }
}
