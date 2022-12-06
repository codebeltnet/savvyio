using Cuemon.Threading;
using System.Threading.Tasks;
using System;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a producer/sender channel used for interacting with other subsystems (out-process/inter-application) to do something (e.g., change the state).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    public interface ISender<in TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Sends the specified <paramref name="message"/> asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SendAsync(IMessage<TRequest> message, Action<AsyncOptions> setup = null);
    }
}
