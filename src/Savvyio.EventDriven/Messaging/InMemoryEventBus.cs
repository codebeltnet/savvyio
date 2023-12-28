using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging
{
    /// <summary>
    /// Provides an in-memory implementation of the <see cref="IPublishSubscribeChannel{TRequest}"/> interface useful for unit testing and the likes thereof.
    /// </summary>
    public class InMemoryEventBus : IPublishSubscribeChannel<IIntegrationEvent>
    {
        private readonly Channel<IMessage<IIntegrationEvent>> _channel = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();

        /// <summary>
        /// Publishes the specified <paramref name="event" /> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="event">The <see cref="IIntegrationEvent" /> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task PublishAsync(IMessage<IIntegrationEvent> @event, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            await _channel.Writer.WriteAsync(@event, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to one or more event(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="asyncHandler">The delegate that will handle the event.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
        {
            var options = setup.Configure();
            await Condition.FlipFlopAsync(options.ThrowIfCancellationWasRequested, () => WaitToReadAsync(asyncHandler, options.CancellationToken), async () =>
            {
                try
                {
                    await WaitToReadAsync(asyncHandler, options.CancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (options.CancellationToken.IsCancellationRequested)
                {
                    // ThrowIfCancellationWasRequested was set to false (default)
                }
            });
        }

        private async Task WaitToReadAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, CancellationToken ct)
        {
            var currentSize = Math.Clamp(_channel.Reader.Count, 0, byte.MaxValue);
            while (currentSize > 0)
            {
                for (var i = 0; i < currentSize; i++)
                {
                    var message = await _channel.Reader.ReadAsync(ct).ConfigureAwait(false);
                    if (message == null) { break; }
                    await asyncHandler.Invoke(message, ct).ConfigureAwait(false);
                }
                currentSize = _channel.Reader.Count;
            }
        }
    }
}
