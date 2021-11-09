namespace Savvyio.Events
{
    public interface IIntegrationEventHandler : IHandler<IIntegrationEvent>
    {
        IHandlerActivator<IIntegrationEvent> IntegrationEvents { get; }
    }
}
