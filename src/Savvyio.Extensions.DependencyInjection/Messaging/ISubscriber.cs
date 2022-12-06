using Cuemon.Extensions.DependencyInjection;
using Savvyio.Messaging;

namespace Savvyio.Extensions.DependencyInjection.Messaging
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of a subscriber/receiver channel used by subsystems to subscribe to messages (typically events) to be made aware of something that has happened.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this channel represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="ISubscriber{TRequest}"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public interface ISubscriber<out TRequest, TMarker> : ISubscriber<TRequest>, IDependencyInjectionMarker<TMarker> where TRequest : IRequest
    {
    }
}
