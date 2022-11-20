using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.Commands.Messaging
{
    /// <summary>
    /// Specifies the consumer/receiver of a bus used used by subsystems to receive a command and perform one or more actions (e.g. change the state).
    /// </summary>
    public interface ICommandReceiver
    {
        /// <summary>
        /// Receive a single command asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        Task<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null);

        /// <summary>
        /// Receive one or more command(s) asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="MessageBatchOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        Task<IEnumerable<IMessage<ICommand>>> ReceiveManyAsync(Action<MessageBatchOptions> setup = null);
    }
}
