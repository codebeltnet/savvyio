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
    /// Provides an in-memory implementation of the <see cref="IPointToPointChannel{TRequest}"/> interface useful for unit testing and the likes thereof.
    /// </summary>
    public class InMemoryCommandQueue : IPointToPointChannel<ICommand>
    {
        private readonly Channel<IMessage<ICommand>> _channel = Channel.CreateUnbounded<IMessage<ICommand>>();

        /// <summary>
        /// Sends the specified <paramref name="messages" /> whose generic type argument is <see cref="ICommand"/> asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="messages">The <see cref="ICommand"/> enclosed messages to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            foreach (var command in messages)
            {
                await _channel.Writer.WriteAsync(command, options.CancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Receive one or more command(s) asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="ICommand"/>.</returns>
        public async IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            await foreach (var message in _channel.Reader.ReadAllAsync(options.CancellationToken).ConfigureAwait(false))
            {
                yield return message;
                if (_channel.Reader.Count == 0) { yield break; }
            }
        }
    }
}
