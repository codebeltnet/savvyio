using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Threading;
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
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting <see cref="IIntegrationEvent"/> implementations to messages.</param>
        /// <param name="options">The <see cref="AmazonEventBusOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public AmazonEventBus(IMarshaller marshaller, AmazonEventBusOptions options) : base(marshaller, options)
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
            var sns = Options.ClientConfigurations.IsValid()
                ? new AmazonSimpleNotificationServiceClient(Options.Credentials, Options.ClientConfigurations.SimpleNotificationService())
                : new AmazonSimpleNotificationServiceClient(Options.Credentials, Options.Endpoint);
            var request = new PublishRequest
            {
                TopicArn = @event.Source,
                Message = await Marshaller.Serialize(@event).ToEncodedStringAsync().ConfigureAwait(false),
                MessageGroupId = UseFirstInFirstOut ? @event.Source : null,
                MessageDeduplicationId = UseFirstInFirstOut ? @event.Id : null,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {
                        MessageAttributeTypeKey, new MessageAttributeValue
                        {
                            DataType = "String",
                            StringValue = @event.Data.GetMemberType()
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

        private async Task InvokeHandlerAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, CancellationToken cancellationToken)
        {
            var hasMessages = true;
            while (hasMessages)
            {
                hasMessages = false;
                await foreach (var message in RetrieveMessagesAsync(cancellationToken).ConfigureAwait(false))
                {
	                hasMessages = true;
	                await asyncHandler.Invoke(message, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
