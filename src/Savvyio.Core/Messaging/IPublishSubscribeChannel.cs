namespace Savvyio.Messaging
{
    /// <summary>
    /// Specifies an interface for a bus that is used for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened).
    /// </summary>
    /// <seealso cref="IPublisher{TRequest}" />
    /// <seealso cref="ISubscriber{TRequest}" />
    public interface IPublishSubscribeChannel<TRequest> : IPublisher<TRequest>, ISubscriber<TRequest> where TRequest : IRequest
    {
    }
}
