using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging
{
    /// <summary>
    /// Provides an in-memory implementation of the <see cref="IIntegrationEventBus"/> interface useful for unit testing and the likes thereof.
    /// </summary>
    /// <seealso cref="IIntegrationEventBus" />
    public class MemoryEventBus : IIntegrationEventBus
    {
        private readonly Channel<IMessage<IIntegrationEvent>> _channel = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

        /// <summary>
        /// Publishes the specified <paramref name="event" /> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="event">The <see cref="IIntegrationEvent" /> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public Task PublishAsync(IMessage<IIntegrationEvent> @event, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return _channel.Writer.WriteAsync(@event, options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Subscribe to one or more event(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="setup">The <see cref="MessageBatchOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="IIntegrationEvent"/>.</returns>
        public async Task<IEnumerable<IMessage<IIntegrationEvent>>> SubscribeAsync(Action<MessageBatchOptions> setup = null)
        {
            var options = setup.Configure();
            var messages = new List<IMessage<IIntegrationEvent>>();
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
