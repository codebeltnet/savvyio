using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Savvyio.Commands;
using Savvyio.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Savvyio.Extensions.RabbitMQ.Commands
{
    /// <summary>
    /// Provides a default implementation of the <see cref="RabbitMqMessage"/> class for messages holding an <see cref="ICommand"/> implementation.
    /// </summary>
    /// <seealso cref="RabbitMqMessage"/>
    /// <seealso cref="IPointToPointChannel{TRequest}"/>
    public class RabbitMqCommandQueue : RabbitMqMessage, IPointToPointChannel<ICommand>
    {
        private readonly RabbitMqCommandQueueOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqCommandQueue"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the RabbitMQ command queue.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public RabbitMqCommandQueue(IMarshaller marshaller, RabbitMqCommandQueueOptions options) : base(marshaller, options)
        {
            Validator.ThrowIfInvalidOptions(options);
            _options = options;
        }

        /// <summary>
        /// Sends the specified command messages asynchronously to the configured RabbitMQ queue.
        /// </summary>
        /// <param name="messages">The messages to send.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            await EnsureConnectivityAsync(options.CancellationToken).ConfigureAwait(false);

            await RabbitMqChannel.QueueDeclareAsync(_options.QueueName, _options.Durable, _options.Exclusive, _options.AutoDelete, cancellationToken: options.CancellationToken).ConfigureAwait(false);

            foreach (var message in messages)
            {
                await RabbitMqChannel.BasicPublishAsync("", _options.QueueName, true, basicProperties: new BasicProperties()
                {
                    Persistent = _options.Persistent,
                    Headers = new Dictionary<string, object>()
                    {
                        { MessageType, message.GetType().ToFullNameIncludingAssemblyName() }
                    }
                }, body: await Marshaller.Serialize(message).ToByteArrayAsync(o => o.CancellationToken = options.CancellationToken).ConfigureAwait(false), options.CancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Receives command messages asynchronously from the configured RabbitMQ queue.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>
        /// An <see cref="IAsyncEnumerable{T}"/> that yields <see cref="IMessage{ICommand}"/> instances as they are received.
        /// </returns>
        public async IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            await EnsureConnectivityAsync(options.CancellationToken).ConfigureAwait(false);

            await RabbitMqChannel.QueueDeclareAsync(_options.QueueName, _options.Durable, _options.Exclusive, _options.AutoDelete, cancellationToken: options.CancellationToken).ConfigureAwait(false);

            var buffer = Channel.CreateUnbounded<IMessage<ICommand>>();
            var consumer = new AsyncEventingBasicConsumer(RabbitMqChannel);
            consumer.ReceivedAsync += async (_, e) =>
            {
                var messageType = Type.GetType((e.BasicProperties.Headers[MessageType] as byte[]).ToEncodedString());
                var deserialized = Marshaller.Deserialize(e.Body.ToArray().ToStream(), messageType) as IMessage<ICommand>;
                deserialized!.Properties.Add(nameof(BasicDeliverEventArgs.DeliveryTag), e.DeliveryTag);
                deserialized.Properties.Add(nameof(CancellationToken), options.CancellationToken);
                deserialized.Properties.Add(nameof(QueueDeclareOk.QueueName), _options.QueueName);
                await buffer.Writer.WriteAsync(deserialized, options.CancellationToken).ConfigureAwait(false);
            };

            await RabbitMqChannel.BasicConsumeAsync(_options.QueueName, autoAck: false, consumer: consumer, cancellationToken: options.CancellationToken).ConfigureAwait(false);

            while (await buffer.Reader.WaitToReadAsync(options.CancellationToken).ConfigureAwait(false))
            {
                while (buffer.Reader.TryRead(out var message))
                {
                    message.Acknowledged += OnMessageAcknowledgedAsync;
                    if (_options.AutoAcknowledge)
                    {
                        await message.AcknowledgeAsync().ConfigureAwait(false);
                    }
                    yield return message;
                }
            }
        }

        private async Task OnMessageAcknowledgedAsync(object sender, AcknowledgedEventArgs e)
        {
            var ct = (CancellationToken)e.Properties[nameof(CancellationToken)];
            var queueName = e.Properties[nameof(QueueDeclareOk.QueueName)] as string;
            await RabbitMqChannel.QueueDeclareAsync(queueName, _options.Durable, _options.Exclusive, _options.AutoDelete, cancellationToken: ct).ConfigureAwait(false);
            await RabbitMqChannel.BasicAckAsync((ulong)e.Properties[nameof(BasicDeliverEventArgs.DeliveryTag)], false, ct).ConfigureAwait(false);
            if (sender is IAcknowledgeable ack) { ack.Acknowledged -= OnMessageAcknowledgedAsync; }
        }
    }
}
