using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Savvyio.EventDriven;
using Savvyio.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Savvyio.Extensions.RabbitMQ.EventDriven
{
    /// <summary>
    /// Represents a RabbitMQ-based implementation of a publish-subscribe event bus for integration events.
    /// </summary>
    public class RabbitMqEventBus : RabbitMqMessage, IPublishSubscribeChannel<IIntegrationEvent>
    {
        private readonly ConnectionFactory _factory;
        private readonly RabbitMqEventBusOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqEventBus"/> class.
        /// </summary>
        /// <param name="marshaller">The marshaller used for serializing and deserializing messages.</param>
        /// <param name="options">The options used to configure the RabbitMQ event bus.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="marshaller"/> cannot be null -or-
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        public RabbitMqEventBus(IMarshaller marshaller, RabbitMqEventBusOptions options) : base(marshaller, options)
        {
            Validator.ThrowIfInvalidOptions(options);
            _options = options;
        }

        /// <summary>
        /// Publishes the specified integration event message asynchronously to the configured RabbitMQ exchange.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task PublishAsync(IMessage<IIntegrationEvent> message, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            await EnsureConnectivityAsync(options.CancellationToken).ConfigureAwait(false);

            await RabbitMqChannel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Fanout, cancellationToken: options.CancellationToken).ConfigureAwait(false);

            await RabbitMqChannel.BasicPublishAsync(_options.ExchangeName, "", false, basicProperties: new BasicProperties()
            {
                Headers = new Dictionary<string, object>()
                    {
                        { MessageType, message.GetType().ToFullNameIncludingAssemblyName() }
                    }
            }, body: await Marshaller.Serialize(message).ToByteArrayAsync(o => o.CancellationToken = options.CancellationToken).ConfigureAwait(false), options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribes to integration event messages from the configured RabbitMQ exchange and invokes the specified asynchronous handler for each received message.
        /// </summary>
        /// <param name="asyncHandler">The function delegate that will handle the message.</param>
        /// <param name="setup">The <see cref="SubscribeAsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            await EnsureConnectivityAsync(options.CancellationToken).ConfigureAwait(false);

            await RabbitMqChannel.ExchangeDeclareAsync(_options.ExchangeName, ExchangeType.Fanout, cancellationToken: options.CancellationToken).ConfigureAwait(false);

            var queue = await RabbitMqChannel.QueueDeclareAsync(cancellationToken: options.CancellationToken).ConfigureAwait(false);

            await RabbitMqChannel.QueueBindAsync(queue.QueueName, _options.ExchangeName, "", cancellationToken: options.CancellationToken).ConfigureAwait(false);

            var buffer = Channel.CreateUnbounded<IMessage<IIntegrationEvent>>();
            var consumer = new AsyncEventingBasicConsumer(RabbitMqChannel);
            consumer.ReceivedAsync += async (_, e) =>
            {
                var messageType = Type.GetType((e.BasicProperties.Headers[MessageType] as byte[]).ToEncodedString());
                var deserialized = Marshaller.Deserialize(e.Body.ToArray().ToStream(), messageType) as IMessage<IIntegrationEvent>;
                deserialized!.Properties.Add(nameof(BasicDeliverEventArgs.DeliveryTag), e.DeliveryTag);
                deserialized.Properties.Add(nameof(CancellationToken), options.CancellationToken);
                deserialized.Properties.Add(nameof(QueueDeclareOk.QueueName), queue.QueueName);
                await buffer.Writer.WriteAsync(deserialized, options.CancellationToken).ConfigureAwait(false);
            };
            
            await RabbitMqChannel.BasicConsumeAsync(queue.QueueName, autoAck: true, consumer: consumer, options.CancellationToken).ConfigureAwait(false);

            while (await buffer.Reader.WaitToReadAsync(options.CancellationToken).ConfigureAwait(false))
            {
                while (buffer.Reader.TryRead(out var message))
                {
                    await asyncHandler(message, options.CancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}
