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
            var js = NatsClient.CreateJetStreamContext();
            foreach (var message in messages)
            {
                await js.PublishAsync(_options.Subject, (await Marshaller.Serialize(message).ToByteArrayAsync().ConfigureAwait(false)).ToBase64String(), headers: new NatsHeaders()
                {
                    { "type", message.GetType().ToFullNameIncludingAssemblyName() }
                });
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
            
            var js = NatsClient.CreateJetStreamContext();
            var stream = await js.CreateOrUpdateStreamAsync(new StreamConfig(streamName, [_options.Subject])
            {
                Retention = StreamConfigRetention.Workqueue
            }, options.CancellationToken).ConfigureAwait(false);
            
            var consumer = await stream.CreateOrUpdateConsumerAsync(new ConsumerConfig(consumerName)
            {
            }, options.CancellationToken).ConfigureAwait(false);

            await foreach (var message in consumer.FetchAsync<string>(opts: new NatsJSFetchOpts()
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
                deserialized.Properties.Add(nameof(NatsJSMsg<string>), message);
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
            var message = (NatsJSMsg<string>)e.Properties[nameof(NatsJSMsg<string>)];
            await message.AckAsync(cancellationToken: ct).ConfigureAwait(false);
            if (sender is IAcknowledgeable ack) { ack.Acknowledged -= OnMessageAcknowledgedAsync; }
        }
    }
}
