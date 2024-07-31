using Cuemon.Extensions.DependencyInjection;
using Savvyio.Messaging;

namespace Savvyio.Extensions.DependencyInjection.Messaging
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of a consumer/receiver channel used by subsystems to receive a command and perform one or more actions (e.g., change the state).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this channel represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IReceiver{TRequest}"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public interface IReceiver<TRequest, TMarker> : IReceiver<TRequest>, IDependencyInjectionMarker<TMarker> where TRequest : IRequest
    {
    }
}
