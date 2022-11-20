using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.Commands.Messaging
{
    /// <summary>
    /// Provides an in-memory implementation of the <see cref="ICommandBus"/> interface useful for unit testing and the likes thereof.
    /// </summary>
    /// <seealso cref="ICommandBus" />
    public class MemoryCommandBus : ICommandBus
    {
        private readonly Channel<IMessage<ICommand>> _channel = Channel.CreateUnbounded<IMessage<ICommand>>();

        /// <summary>
        /// Sends the specified <paramref name="command" /> asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="command">The enclosed <see cref="ICommand" /> to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public Task SendAsync(IMessage<ICommand> command, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return _channel.Writer.WriteAsync(command, options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Receive a single command asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        public Task<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return _channel.Reader.ReadAsync(options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Receive one or more command(s) asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="MessageBatchOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        public async Task<IEnumerable<IMessage<ICommand>>> ReceiveManyAsync(Action<MessageBatchOptions> setup = null)
        {
            var options = setup.Configure();
            var messages = new List<IMessage<ICommand>>();
            var requested = Math.Min(_channel.Reader.Count, options.MaxNumberOfMessages);
            for (var i = 0; i < requested; i++)
            {
                var message = await _channel.Reader.ReadAsync(options.CancellationToken).ConfigureAwait(false);
                messages.Add(message);
            }
            return messages;
        }
    }
}
