using Cuemon.Extensions.DependencyInjection;
using Savvyio.Messaging;

namespace Savvyio.Extensions.DependencyInjection.Messaging
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of a producer/sender channel used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this channel represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="ISender{TRequest}"/>
    /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
    public interface ISender<in TRequest, TMarker> : ISender<TRequest>, IDependencyInjectionMarker<TMarker> where TRequest : IRequest
    {
    }
}
