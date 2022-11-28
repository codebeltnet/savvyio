using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Threading;
using Microsoft.Extensions.Options;
using Savvyio.EventDriven;
using Savvyio.Messaging;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    /// <summary>
    /// Provides a default implementation of the <see cref="AmazonBus{TRequest}"/> class tailored for messages holding an <see cref="IIntegrationEvent"/> implementation.
    /// </summary>
    /// <seealso cref="AmazonBus{TRequest}" />
    public class AmazonEventBus : AmazonBus<IIntegrationEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonEventBus"/> class.
        /// </summary>
        /// <param name="options">The <see cref="AmazonMessageOptions" /> which need to be configured.</param>
        public AmazonEventBus(IOptions<AmazonMessageOptions> options) : base(options.Value)
        {
        }

        /// <summary>
        /// Publishes the specified <paramref name="event" /> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="event">The event to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override async Task PublishAsync(IMessage<IIntegrationEvent> @event, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            var sns = new AmazonSimpleNotificationServiceClient(Options.Credentials, Options.Endpoint);
            var request = new PublishRequest
            {
                TargetArn = @event.Source,
                Message = await JsonFormatter.SerializeObject(@event).ToEncodedStringAsync().ConfigureAwait(false),
                MessageGroupId = @event.Source,
                MessageDeduplicationId = @event.Id,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        MessageAttributeTypeKey, new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = @event.Type
                        }
                    }
                }
            };

            await sns.PublishAsync(request, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to one or more message(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle the message.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override async Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
        {
            var options = setup.Configure();

            await Condition.FlipFlopAsync(options.ThrowIfCancellationWasRequested, () => InvokeHandlerAsync(asyncHandler, options.CancellationToken), async () =>
            {
                try
                {
                    await InvokeHandlerAsync(asyncHandler, options.CancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (options.CancellationToken.IsCancellationRequested)
                {
                    // ThrowIfCancellationWasRequested was set to false (default)
                }
            });
        }

        private async Task InvokeHandlerAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, CancellationToken ct)
        {
            var hasMessages = true;
            while (hasMessages)
            {
                hasMessages = false;
                var messages = await RetrieveMessagesAsync(o =>
                {
                    o.MaxNumberOfMessages = int.MaxValue;
                    o.CancellationToken = ct;
                }).ConfigureAwait(false);

                foreach (var message in messages)
                {
                    hasMessages = true;
                    await asyncHandler.Invoke(message, ct).ConfigureAwait(false);
                }
            }
        }
    }
}
