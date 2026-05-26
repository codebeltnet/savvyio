using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using NATS.Client.Core;
using Savvyio.EventDriven;
using Savvyio.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Extensions.NATS.EventDriven
{
    /// <summary>
    /// Provides a NATS implementation of the <see cref="IPublishSubscribeChannel{TRequest}"/> for integration event messages.
    /// </summary>
    /// <seealso cref="NatsMessage"/>
    /// <seealso cref="IPublishSubscribeChannel{TRequest}"/>
    public class NatsEventBus : NatsMessage, IPublishSubscribeChannel<IIntegrationEvent>
    {
        private readonly NatsEventBusOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="NatsEventBus"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The <see cref="NatsEventBusOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public NatsEventBus(IMarshaller marshaller, NatsEventBusOptions options) : base(marshaller, options)
        {
            Validator.ThrowIfInvalidOptions(options);
            _options = options;
        }

        /// <summary>
        /// Publishes the specified integration event message asynchronously to the configured NATS subject.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task PublishAsync(IMessage<IIntegrationEvent> message, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            await PublishMessageAsync(_options.Subject, (await Marshaller.Serialize(message).ToByteArrayAsync().ConfigureAwait(false)).ToBase64String(), new NatsHeaders()
                {
                    { "type", message.GetType().ToFullNameIncludingAssemblyName() }
                }, options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to integration event messages from the configured NATS subject and invokes the specified asynchronous handler for each received message.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle the message.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            await foreach (var message in SubscribeMessagesAsync(_options.Subject, new NatsSubOpts()
                           {
                           }, cancellationToken: options.CancellationToken))
            {
                var messageType = Type.GetType(message.Headers["type"]);
                var deserialized = Marshaller.Deserialize(await message.Data.FromBase64().ToStreamAsync().ConfigureAwait(false), messageType) as IMessage<IIntegrationEvent>;
                await asyncHandler.Invoke(deserialized, options.CancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Publishes a serialized message to NATS.
        /// </summary>
        /// <param name="subject">The subject to publish to.</param>
        /// <param name="message">The serialized message payload.</param>
        /// <param name="headers">The message headers.</param>
        /// <param name="cancellationToken">The cancellation token of the asynchronous operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async Task PublishMessageAsync(string subject, string message, NatsHeaders headers, CancellationToken cancellationToken)
        {
            await NatsClient.PublishAsync(subject, message, headers: headers, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to NATS messages.
        /// </summary>
        /// <param name="subject">The subject to subscribe to.</param>
        /// <param name="options">The subscription options.</param>
        /// <param name="cancellationToken">The cancellation token of the asynchronous operation.</param>
        /// <returns>An asynchronous sequence of received NATS messages.</returns>
        protected virtual async IAsyncEnumerable<ReceivedNatsMessage> SubscribeMessagesAsync(string subject, NatsSubOpts options, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var message in NatsClient.SubscribeAsync<string>(subject, opts: options, cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                yield return new ReceivedNatsMessage(message.Headers, message.Data);
            }
        }

        /// <summary>
        /// Represents a NATS message received from a subscription.
        /// </summary>
        protected sealed class ReceivedNatsMessage
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ReceivedNatsMessage"/> class.
            /// </summary>
            /// <param name="headers">The NATS message headers.</param>
            /// <param name="data">The NATS message payload.</param>
            public ReceivedNatsMessage(NatsHeaders headers, string data)
            {
                Headers = headers;
                Data = data;
            }

            /// <summary>
            /// Gets the NATS message headers.
            /// </summary>
            public NatsHeaders Headers { get; }

            /// <summary>
            /// Gets the NATS message payload.
            /// </summary>
            public string Data { get; }
        }
    }
}
