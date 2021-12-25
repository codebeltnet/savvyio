namespace Savvyio.Events
{
    /// <summary>
    /// Specifies a handler resposible for objects that implements the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    /// <seealso cref="IFireForgetHandler{TRequest}" />
    public interface IIntegrationEventHandler : IFireForgetHandler<IIntegrationEvent>
    {
    }
}
