using Savvyio.Messaging;

namespace Savvyio.Extensions.DependencyInjection.Messaging
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of a bus that is used for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this bus represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IPublishSubscribeChannel{TRequest}"/>
    /// <seealso cref="IPublisher{TRequest,TMarker}"/>
    /// <seealso cref="ISubscriber{TRequest,TMarker}"/>
    public interface IPublishSubscribeChannel<TRequest, TMarker> : IPublishSubscribeChannel<TRequest>, IPublisher<TRequest, TMarker>, ISubscriber<TRequest, TMarker> where TRequest : IRequest
    {
    }
}
