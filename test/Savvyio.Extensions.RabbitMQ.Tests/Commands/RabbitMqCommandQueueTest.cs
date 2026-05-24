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
using Savvyio.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.RabbitMQ.Commands
{
    public class RabbitMqCommandQueueTest : Test
    {
        public RabbitMqCommandQueueTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Should_Throw_When_Marshaller_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new RabbitMqCommandQueue(null, CreateOptions()));
        }

        [Fact]
        public void Constructor_Should_Throw_When_Options_Are_Invalid()
        {
            Assert.Throws<ArgumentException>(() => new RabbitMqCommandQueue(JsonMarshaller.Default, new RabbitMqCommandQueueOptions()));
        }

        [Fact]
        public async Task SendAsync_Should_Declare_Queue_And_Publish_Each_Message()
        {
            var options = CreateOptions(o => o.Persistent = true);
            var channel = new Mock<IChannel>();
            BasicProperties publishedProperties = null;

            channel.Setup(c => c.QueueDeclareAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueueDeclareOk(options.QueueName, 0, 0));
            channel.Setup(c => c.BasicPublishAsync("", options.QueueName, true, It.IsAny<BasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()))
                .Callback<string, string, bool, BasicProperties, ReadOnlyMemory<byte>, CancellationToken>((_, _, _, properties, _, _) => publishedProperties = properties)
                .Returns(ValueTask.CompletedTask);

            var sut = CreateSut(channel.Object, options);
            var messages = new IMessage<ICommand>[]
            {
                new Message<TestCommand>("id-1", new Uri("urn:command:test"), "command.test", new TestCommand(), DateTime.UtcNow),
                new Message<TestCommand>("id-2", new Uri("urn:command:test"), "command.test", new TestCommand(), DateTime.UtcNow)
            };

            await sut.SendAsync(messages);

            channel.Verify(c => c.QueueDeclareAsync(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, null, false, false, It.IsAny<CancellationToken>()), Times.Once);
            channel.Verify(c => c.BasicPublishAsync("", options.QueueName, true, It.IsAny<BasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
            Assert.True(publishedProperties.Persistent);
            Assert.Equal($"{typeof(Message<TestCommand>).FullName}, {typeof(Message<TestCommand>).Assembly.GetName().Name}", publishedProperties.Headers["type"]);
        }

        [Fact]
        public async Task ReceiveAsync_Should_Acknowledge_Message_When_Requested()
        {
            var options = CreateOptions();
            var channel = new Mock<IChannel>();
            var consumerSource = new TaskCompletionSource<IAsyncBasicConsumer>();

            channel.Setup(c => c.QueueDeclareAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueueDeclareOk(options.QueueName, 0, 0));
            channel.Setup(c => c.BasicConsumeAsync(options.QueueName, false, string.Empty, false, false, null, It.IsAny<IAsyncBasicConsumer>(), It.IsAny<CancellationToken>()))
                .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IAsyncBasicConsumer, CancellationToken>((_, _, _, _, _, _, consumer, _) => consumerSource.TrySetResult(consumer))
                .ReturnsAsync("consumer-tag");
            channel.Setup(c => c.BasicAckAsync(77UL, false, It.IsAny<CancellationToken>())).Returns(ValueTask.CompletedTask);

            var sut = CreateSut(channel.Object, options);
            await using var enumerator = sut.ReceiveAsync().GetAsyncEnumerator();
            var moveNext = enumerator.MoveNextAsync().AsTask();
            var consumer = await consumerSource.Task.ConfigureAwait(false);

            await PublishDeliveredMessageAsync((AsyncEventingBasicConsumer)consumer, new Message<TestCommand>("id-1", new Uri("urn:command:test"), "command.test", new TestCommand(), DateTime.UtcNow), 77UL, options.QueueName).ConfigureAwait(false);

            Assert.True(await moveNext.ConfigureAwait(false));
            var message = enumerator.Current;

            Assert.Equal(77UL, message.Properties[nameof(BasicDeliverEventArgs.DeliveryTag)]);
            Assert.Equal(options.QueueName, message.Properties[nameof(QueueDeclareOk.QueueName)]);

            await message.AcknowledgeAsync().ConfigureAwait(false);

            channel.Verify(c => c.BasicAckAsync(77UL, false, It.IsAny<CancellationToken>()), Times.Once);
            channel.Verify(c => c.QueueDeclareAsync(options.QueueName, options.Durable, options.Exclusive, options.AutoDelete, null, false, false, It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ReceiveAsync_Should_Auto_Acknowledge_Message_When_Configured()
        {
            var options = CreateOptions(o => o.AutoAcknowledge = true);
            var channel = new Mock<IChannel>();
            var consumerSource = new TaskCompletionSource<IAsyncBasicConsumer>();

            channel.Setup(c => c.QueueDeclareAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new QueueDeclareOk(options.QueueName, 0, 0));
            channel.Setup(c => c.BasicConsumeAsync(options.QueueName, false, string.Empty, false, false, null, It.IsAny<IAsyncBasicConsumer>(), It.IsAny<CancellationToken>()))
                .Callback<string, bool, string, bool, bool, IDictionary<string, object>, IAsyncBasicConsumer, CancellationToken>((_, _, _, _, _, _, consumer, _) => consumerSource.TrySetResult(consumer))
                .ReturnsAsync("consumer-tag");
            channel.Setup(c => c.BasicAckAsync(88UL, false, It.IsAny<CancellationToken>())).Returns(ValueTask.CompletedTask);

            var sut = CreateSut(channel.Object, options);
            await using var enumerator = sut.ReceiveAsync().GetAsyncEnumerator();
            var moveNext = enumerator.MoveNextAsync().AsTask();
            var consumer = await consumerSource.Task.ConfigureAwait(false);

            await PublishDeliveredMessageAsync((AsyncEventingBasicConsumer)consumer, new Message<TestCommand>("id-2", new Uri("urn:command:test"), "command.test", new TestCommand(), DateTime.UtcNow), 88UL, options.QueueName).ConfigureAwait(false);

            Assert.True(await moveNext.ConfigureAwait(false));
            channel.Verify(c => c.BasicAckAsync(88UL, false, It.IsAny<CancellationToken>()), Times.Once);
        }

        private static RabbitMqCommandQueue CreateSut(IChannel channel, RabbitMqCommandQueueOptions options)
        {
            var connection = new Mock<IConnection>();
            connection.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions?>(), It.IsAny<CancellationToken>())).ReturnsAsync(channel);

            var factory = new Mock<IConnectionFactory>();
            factory.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(connection.Object);

            var sut = new RabbitMqCommandQueue(JsonMarshaller.Default, options);
            typeof(RabbitMqMessage)
                .GetProperty("RabbitMqFactory", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(sut, factory.Object);
            return sut;
        }

        private static RabbitMqCommandQueueOptions CreateOptions(Action<RabbitMqCommandQueueOptions> setup = null)
        {
            var options = new RabbitMqCommandQueueOptions
            {
                QueueName = "orders"
            };
            setup?.Invoke(options);
            return options;
        }

        private static async Task PublishDeliveredMessageAsync(AsyncEventingBasicConsumer consumer, Message<TestCommand> message, ulong deliveryTag, string queueName)
        {
            var body = await JsonMarshaller.Default.Serialize(message).ToByteArrayAsync().ConfigureAwait(false);
            var properties = new BasicProperties
            {
                Headers = new Dictionary<string, object>
                {
                    ["type"] = Encoding.UTF8.GetBytes($"{typeof(Message<TestCommand>).FullName}, {typeof(Message<TestCommand>).Assembly.GetName().Name}")
                }
            };

            await consumer.HandleBasicDeliverAsync("consumer-tag", deliveryTag, false, string.Empty, queueName, properties, body, CancellationToken.None).ConfigureAwait(false);
        }

        private sealed class TestCommand : ICommand
        {
            public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
        }
    }
}
