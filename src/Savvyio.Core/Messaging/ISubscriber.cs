using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a subscriber/receiver channel used by subsystems to subscribe to messages (typically events) to be made aware of something that has happened.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    public interface ISubscriber<out TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Subscribe to one or more message(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle the message.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task SubscribeAsync(Func<IMessage<TRequest>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null);
    }
}
