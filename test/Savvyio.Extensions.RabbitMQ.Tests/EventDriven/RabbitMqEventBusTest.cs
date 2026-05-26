using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.RabbitMQ.EventDriven
{
    public class RabbitMqEventBusTest : Test
    {
        public RabbitMqEventBusTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Should_Throw_When_Marshaller_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new RabbitMqEventBus(null, CreateOptions()));
        }

        [Fact]
        public void Constructor_Should_Throw_When_Options_Are_Invalid()
        {
            Assert.Throws<ArgumentException>(() => new RabbitMqEventBus(JsonMarshaller.Default, new RabbitMqEventBusOptions()));
        }

        [Fact]
        public async Task PublishAsync_Should_Declare_Exchange_And_Publish_Message()
        {
            var options = CreateOptions(o => o.Persistent = true);
            var channel = new Mock<IChannel>();
            BasicProperties publishedProperties = null;

            channel.Setup(c => c.ExchangeDeclareAsync(options.ExchangeName, ExchangeType.Fanout, false, false, null, false, false, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            channel.Setup(c => c.BasicPublishAsync(options.ExchangeName, string.Empty, false, It.IsAny<BasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
                .Callback<string, string, bool, BasicProperties, ReadOnlyMemory<byte>, CancellationToken>((_, _, _, properties, _, _) => publishedProperties = properties)
                .Returns(ValueTask.CompletedTask);

            var sut = CreateSut(channel.Object, options);
            var message = new Message<TestIntegrationEvent>("id-1", new Uri("urn:event:test"), "event.test", new TestIntegrationEvent(), DateTime.UtcNow);

            await sut.PublishAsync(message);

            channel.Verify(c => c.ExchangeDeclareAsync(options.ExchangeName, ExchangeType.Fanout, false, false, null, false, false, It.IsAny<CancellationToken>()), Times.Once);
            channel.Verify(c => c.BasicPublishAsync(options.ExchangeName, string.Empty, false, It.IsAny<BasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(publishedProperties.Persistent);
            Assert.Equal($"{typeof(Message<TestIntegrationEvent>).FullName}, {typeof(Message<TestIntegrationEvent>).Assembly.GetName().Name}", publishedProperties.Headers["type"]);
        }

        [Fact]
        public async Task SubscribeAsync_Should_Invoke_Handler_For_Received_Message()
        {
            var options = CreateOptions();
            var channel = new Mock<IChannel>();
            var consumerSource = new TaskCompletionSource<IAsyncBasicConsumer>();
            var handled = 0;
            IMessage<IIntegrationEvent> received = null;
            using var cts = new CancellationTokenSource();

            channel.Setup(c => c.ExchangeDeclareAsync(options.ExchangeName, ExchangeType.Fanout, false, false, null, false, false, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            channel.Setup(c => c.QueueDeclareAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueueDeclareOk("events-queue", 0, 0));
            channel.Setup(c => c.QueueBindAsync("events-queue", options.ExchangeName, string.Empty, null, false, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            channel.Setup(c => c.BasicConsumeAsync("events-queue", true, string.Empty, false, false, null, It.IsAny<IAsyncBasicConsumer>(), It.IsAny<CancellationToken>()))
                .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IAsyncBasicConsumer, CancellationToken>((_, _, _, _, _, _, consumer, _) => consumerSource.TrySetResult(consumer))
                .ReturnsAsync("consumer-tag");

            var sut = CreateSut(channel.Object, options);
            var subscribeTask = sut.SubscribeAsync((message, token) =>
            {
                handled++;
                received = message;
                cts.Cancel();
                return Task.CompletedTask;
            }, o => o.CancellationToken = cts.Token);

            var consumer = await consumerSource.Task.ConfigureAwait(false);
            await PublishDeliveredMessageAsync((AsyncEventingBasicConsumer)consumer, new Message<TestIntegrationEvent>("id-2", new Uri("urn:event:test"), "event.test", new TestIntegrationEvent(), DateTime.UtcNow), 91UL).ConfigureAwait(false);

            try
            {
                await subscribeTask.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // expected: the subscription loop exits when the token is cancelled inside the handler
            }

            Assert.Equal(1, handled);
            Assert.NotNull(received);
            Assert.Equal(91UL, received.Properties[nameof(BasicDeliverEventArgs.DeliveryTag)]);
            Assert.Equal("events-queue", received.Properties[nameof(QueueDeclareOk.QueueName)]);
        }

        private static RabbitMqEventBus CreateSut(IChannel channel, RabbitMqEventBusOptions options)
        {
            var connection = new Mock<IConnection>();
            connection.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions?>(), It.IsAny<CancellationToken>())).ReturnsAsync(channel);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(connection.Object);

            var sut = new RabbitMqEventBus(JsonMarshaller.Default, options);
            typeof(RabbitMqMessage)
                .GetProperty("RabbitMqFactory", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(sut, factory.Object);
            return sut;
        }

        private static RabbitMqEventBusOptions CreateOptions(Action<RabbitMqEventBusOptions> setup = null)
        {
            var options = new RabbitMqEventBusOptions
            {
                ExchangeName = "events"
            };
            setup?.Invoke(options);
            return options;
        }

        private static async Task PublishDeliveredMessageAsync(AsyncEventingBasicConsumer consumer, Message<TestIntegrationEvent> message, ulong deliveryTag)
        {
            var body = await JsonMarshaller.Default.Serialize(message).ToByteArrayAsync().ConfigureAwait(false);
            var properties = new BasicProperties
            {
                Headers = new Dictionary<string, object>
                {
                    ["type"] = Encoding.UTF8.GetBytes($"{typeof(Message<TestIntegrationEvent>).FullName}, {typeof(Message<TestIntegrationEvent>).Assembly.GetName().Name}")
                }
            };

            await consumer.HandleBasicDeliverAsync("consumer-tag", deliveryTag, false, string.Empty, string.Empty, properties, body, CancellationToken.None).ConfigureAwait(false);
        }

        private sealed class TestIntegrationEvent : IIntegrationEvent
        {
            public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
        }
    }
}
