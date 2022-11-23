using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a publisher/sender channel for interacting with other subsystems (out-process/inter-application) to be notified (e.g., made aware of something that has happened).
    /// </summary>
    public interface IPublisher<in TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Publishes the specified <paramref name="message"/> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task PublishAsync(IMessage<TRequest> message, Action<AsyncOptions> setup = null);
    }
}
