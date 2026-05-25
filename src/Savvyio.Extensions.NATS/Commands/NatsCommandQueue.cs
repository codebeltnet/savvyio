using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Net;
using Savvyio.Commands;
using Savvyio.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Extensions.NATS.Commands
{
    /// <summary>
    /// Provides a NATS JetStream implementation of the <see cref="IPointToPointChannel{TRequest}"/> for command messages.
    /// </summary>
    /// <seealso cref="NatsMessage"/>
    /// <seealso cref="IPointToPointChannel{TRequest}"/>
    public class NatsCommandQueue : NatsMessage, IPointToPointChannel<ICommand>
    {
        private readonly NatsCommandQueueOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="NatsCommandQueue"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The <see cref="NatsCommandQueueOptions"/> used to configure this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null - or -
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public NatsCommandQueue(IMarshaller marshaller, NatsCommandQueueOptions options) : base(marshaller, options)
        {
            Validator.ThrowIfInvalidOptions(options);
            _options = options;
        }

        /// <summary>
        /// Sends the specified command messages asynchronously to the configured NATS JetStream subject.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
        {
            foreach (var message in messages)
            {
                await PublishMessageAsync(_options.Subject, (await Marshaller.Serialize(message).ToByteArrayAsync().ConfigureAwait(false)).ToBase64String(), new NatsHeaders()
                {
                    { "type", message.GetType().ToFullNameIncludingAssemblyName() }
                }).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Receives command messages asynchronously from the configured NATS JetStream consumer.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>
        /// An <see cref="IAsyncEnumerable{T}"/> that yields <see cref="IMessage{ICommand}"/> instances as they are received.
        /// </returns>
        public async IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            
            var streamName = _options.StreamName;
            var consumerName = _options.ConsumerName;
            
            var consumer = await CreateConsumerAsync(new StreamConfig(streamName, [_options.Subject])
            {
                Retention = StreamConfigRetention.Workqueue
            }, new ConsumerConfig(consumerName)
            {
            }, options.CancellationToken).ConfigureAwait(false);

            await foreach (var message in FetchMessagesAsync(consumer, new NatsJSFetchOpts()
                           {
                               Expires = _options.Expires == TimeSpan.Zero ? null : _options.Expires,
                               MaxMsgs = _options.MaxMessages,
                               IdleHeartbeat = _options.Heartbeat == TimeSpan.Zero ? null : _options.Heartbeat
            }, cancellationToken: options.CancellationToken))
            {
                options.CancellationToken.ThrowIfCancellationRequested();
                var messageType = Type.GetType(message.Headers["type"]);
                var deserialized = Marshaller.Deserialize(await message.Data.FromBase64().ToStreamAsync().ConfigureAwait(false), messageType) as IMessage<ICommand>;
                deserialized!.Properties.Add(nameof(CancellationToken), options.CancellationToken);
                deserialized.Properties.Add(nameof(ReceivedNatsMessage), message);
                deserialized.Acknowledged += OnMessageAcknowledgedAsync;
                if (_options.AutoAcknowledge)
                {
                    await deserialized.AcknowledgeAsync().ConfigureAwait(false);
                }
                yield return deserialized;
            }
        }

        /// <summary>
        /// Handles the acknowledgment of a message by sending an acknowledgment to the NATS JetStream server.
        /// </summary>
        /// <param name="sender">The message that was acknowledged.</param>
        /// <param name="e">The event arguments containing properties of the acknowledged message.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        private async Task OnMessageAcknowledgedAsync(object sender, AcknowledgedEventArgs e)
        {
            var ct = (CancellationToken)e.Properties[nameof(CancellationToken)];
            var message = (ReceivedNatsMessage)e.Properties[nameof(ReceivedNatsMessage)];
            await message.AcknowledgeAsync(ct).ConfigureAwait(false);
            if (sender is IAcknowledgeable ack) { ack.Acknowledged -= OnMessageAcknowledgedAsync; }
        }

        /// <summary>
        /// Publishes a serialized message to NATS JetStream.
        /// </summary>
        /// <param name="subject">The subject to publish to.</param>
        /// <param name="message">The serialized message payload.</param>
        /// <param name="headers">The message headers.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual async Task PublishMessageAsync(string subject, string message, NatsHeaders headers)
        {
            await CreateJetStreamContext().PublishAsync(subject, message, headers: headers).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates or updates the stream and consumer used by receive operations.
        /// </summary>
        /// <param name="streamConfig">The stream configuration.</param>
        /// <param name="consumerConfig">The consumer configuration.</param>
        /// <param name="cancellationToken">The cancellation token of the asynchronous operation.</param>
        /// <returns>A consumer that can fetch messages.</returns>
        protected virtual async Task<INatsJSConsumer> CreateConsumerAsync(StreamConfig streamConfig, ConsumerConfig consumerConfig, CancellationToken cancellationToken)
        {
            var context = CreateJetStreamContext();
            await context.CreateOrUpdateStreamAsync(streamConfig, cancellationToken).ConfigureAwait(false);
            return await context.CreateOrUpdateConsumerAsync(streamConfig.Name, consumerConfig, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a NATS JetStream context used by send and receive operations.
        /// </summary>
        /// <returns>An <see cref="INatsJSContext"/> for JetStream operations.</returns>
        protected virtual INatsJSContext CreateJetStreamContext()
        {
            return NatsClient.CreateJetStreamContext();
        }

        /// <summary>
        /// Fetches messages from the specified NATS JetStream consumer.
        /// </summary>
        /// <param name="consumer">The consumer to fetch messages from.</param>
        /// <param name="options">The fetch options.</param>
        /// <param name="cancellationToken">The cancellation token of the asynchronous operation.</param>
        /// <returns>An asynchronous sequence of received NATS messages.</returns>
        protected virtual async IAsyncEnumerable<ReceivedNatsMessage> FetchMessagesAsync(INatsJSConsumer consumer, NatsJSFetchOpts options, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var message in consumer.FetchAsync<string>(opts: options, cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                yield return new ReceivedNatsMessage(message.Headers, message.Data, ct => message.AckAsync(cancellationToken: ct).AsTask());
            }
        }

        /// <summary>
        /// Represents a NATS message received from JetStream.
        /// </summary>
        protected sealed class ReceivedNatsMessage
        {
            private readonly Func<CancellationToken, Task> _acknowledgeAsync;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReceivedNatsMessage"/> class.
            /// </summary>
            /// <param name="headers">The NATS message headers.</param>
            /// <param name="data">The NATS message payload.</param>
            /// <param name="acknowledgeAsync">The delegate used to acknowledge the message.</param>
            public ReceivedNatsMessage(NatsHeaders headers, string data, Func<CancellationToken, Task> acknowledgeAsync)
            {
                Headers = headers;
                Data = data;
                _acknowledgeAsync = acknowledgeAsync;
            }

            /// <summary>
            /// Gets the NATS message headers.
            /// </summary>
            public NatsHeaders Headers { get; }

            /// <summary>
            /// Gets the NATS message payload.
            /// </summary>
            public string Data { get; }

            /// <summary>
            /// Acknowledges the message.
            /// </summary>
            /// <param name="cancellationToken">The cancellation token of the asynchronous operation.</param>
            /// <returns>A task that represents the asynchronous operation.</returns>
            public Task AcknowledgeAsync(CancellationToken cancellationToken)
            {
                return _acknowledgeAsync(cancellationToken);
            }
        }
    }
}
