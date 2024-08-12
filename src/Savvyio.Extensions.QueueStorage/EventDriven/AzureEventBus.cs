using System;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.Extensions.QueueStorage.EventDriven
{
    /// <summary>
    /// Provides a combined Azure Event Grid/Azure Storage Queue implementation of the <see cref="IPublishSubscribeChannel{TRequest}"/>.
    /// </summary>
    public class AzureEventBus : AzureQueue<IIntegrationEvent>, IPublishSubscribeChannel<IIntegrationEvent>
    {
        private readonly EventGridPublisherClient _client;
        private readonly IMarshaller _marshaller;

        /// <summary>
        /// The attribute name for the CloudEvent type extension.
        /// </summary>
        public const string CloudEventTypeExtensionAttribute = "typeext";

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureEventBus"/> class.
        /// </summary>
        /// <param name="marshaller">The <see cref="IMarshaller"/> to use for serializing and deserializing <see cref="IIntegrationEvent"/> implementations to messages.</param>
        /// <param name="azureQueueOptions">The <see cref="AzureQueueOptions"/> for configuring <see cref="AzureQueue{TRequest}"/>.</param>
        /// <param name="azureEventBusOptions">The <see cref="AzureEventBusOptions"/> used to configure this instance.</param>
        public AzureEventBus(IMarshaller marshaller, AzureQueueOptions azureQueueOptions, AzureEventBusOptions azureEventBusOptions) : base(marshaller, azureQueueOptions, receiveMessageFormatter: (rawMessage, serializer) =>
        {
            var json = rawMessage.MessageText;
            var jsonStream = json.FromBase64().ToStream();
            var type = JsonNode.Parse(jsonStream.ToEncodedString(o => o.LeaveOpen = true))!.Root[CloudEventTypeExtensionAttribute]!.GetValue<string>();
            return serializer.Deserialize(jsonStream, Type.GetType(type)!) as IMessage<IIntegrationEvent>;
        })
        {
            _marshaller = marshaller;

            if (azureEventBusOptions.Credential != null)
            {
                _client = new EventGridPublisherClient(azureEventBusOptions.TopicEndpoint, azureEventBusOptions.Credential, azureEventBusOptions.Settings);
            }
            else if (azureEventBusOptions.SasCredential != null)
            {
                _client = new EventGridPublisherClient(azureEventBusOptions.TopicEndpoint, azureEventBusOptions.SasCredential, azureEventBusOptions.Settings);
            }
            else if (azureEventBusOptions.KeyCredential != null)
            {
                _client = new EventGridPublisherClient(azureEventBusOptions.TopicEndpoint, azureEventBusOptions.KeyCredential, azureEventBusOptions.Settings);
            }
        }

        /// <summary>
        /// Publishes the specified <paramref name="message" /> asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null.
        /// </exception>
        public async Task PublishAsync(IMessage<IIntegrationEvent> message, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(message);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            var ce = message.ToCloudEvent();

            if (message is ISignedMessage<IIntegrationEvent> signedMessage) { ce.Add(nameof(ISignedMessage<IIntegrationEvent>.Signature), signedMessage.Signature); }

            ce.Add(CloudEventTypeExtensionAttribute, message.GetType().ToFullNameIncludingAssemblyName());

            var json = _marshaller.Serialize(ce).ToEncodedString();
            await _client.SendEventAsync(Azure.Messaging.CloudEvent.Parse(new BinaryData(json)), options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to one or more message(s) asynchronous using Publish-Subscribe Channel/Pub-Sub MEP.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle received messages.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asyncHandler"/> cannot be null.
        /// </exception>
        public async Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(asyncHandler);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

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
            }).ConfigureAwait(false);
        }

        private async Task InvokeHandlerAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, CancellationToken cancellationToken)
        {
            var hasMessages = true;
            while (hasMessages)
            {
                hasMessages = false;
                await foreach (var message in ReceiveMessagesAsync(o => o.CancellationToken = cancellationToken).ConfigureAwait(false))
                {
                    hasMessages = true;
                    await asyncHandler.Invoke(message, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
