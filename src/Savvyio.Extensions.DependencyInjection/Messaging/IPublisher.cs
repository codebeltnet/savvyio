using Cuemon.Extensions.DependencyInjection;
using Savvyio.Messaging;

namespace Savvyio.Extensions.DependencyInjection.Messaging
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of a publisher/sender channel for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this channel represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IPublisher{TRequest}"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public interface IPublisher<in TRequest, TMarker> : IPublisher<TRequest>, IDependencyInjectionMarker<TMarker> where TRequest : IRequest
    {
    }
}
