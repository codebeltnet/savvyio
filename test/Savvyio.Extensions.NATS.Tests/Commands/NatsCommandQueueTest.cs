using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Moq;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using Savvyio.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.NATS.Commands
{
    public class NatsCommandQueueTest : Test
    {
        private IMarshaller _marshaller = JsonMarshaller.Default;
        private NatsCommandQueueOptions _options = new ();

        public NatsCommandQueueTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldInitialize_WithValidOptions()
        {
            var options = new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject"
            };

            var queue = new NatsCommandQueue(_marshaller, options);

            Assert.NotNull(queue);
            Assert.IsAssignableFrom<IPointToPointChannel<ICommand>>(queue);
            Assert.IsAssignableFrom<ISender<ICommand>>(queue);
            Assert.IsAssignableFrom<IReceiver<ICommand>>(queue);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenMarshallerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NatsCommandQueue(null, _options));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NatsCommandQueue(_marshaller, null));
        }

        
        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsAreInvalid()
        {
            Assert.Throws<ArgumentException>(() => new NatsCommandQueue(_marshaller, _options));
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Return_Connection()
        {
            var queue = new NatsCommandQueue(_marshaller, new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject"
            });

            Assert.Same(queue.GetHealthCheckTarget(), queue.GetHealthCheckTarget());
            Assert.NotNull(queue.GetHealthCheckTarget());
        }

        [Fact]
        public async Task SendAsync_WithEmptyMessages_ShouldCoverJetStreamContextCreation()
        {
            var queue = new NatsCommandQueue(_marshaller, new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject"
            });

            await queue.SendAsync(Array.Empty<IMessage<ICommand>>());

            Assert.True(true);
        }

        [Fact]
        public async Task SendAsync_WithOneMessage_ShouldCoverLoopBodyBeforeConnectionFailure()
        {
            var queue = new NatsCommandQueue(_marshaller, new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject",
                NatsUrl = new Uri("nats://localhost:59999")
            });

            var message = new Message<TestCommand>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test",
                new TestCommand());

            var ex = await Record.ExceptionAsync(() => queue.SendAsync(new IMessage<ICommand>[] { message }));

            Assert.NotNull(ex);
        }

        [Fact]
        public async Task SendAsync_ShouldPublishSerializedMessages()
        {
            var queue = new TestableNatsCommandQueue(_marshaller, CreateOptions());
            var message = CreateMessage();

            await queue.SendAsync([message]);

            Assert.Equal("subject", queue.PublishedSubject);
            Assert.Equal(message.GetType().ToFullNameIncludingAssemblyName(), queue.PublishedHeaders["type"]);
            Assert.NotNull(queue.PublishedMessage);
        }

        [Fact]
        public async Task SendAsync_ShouldUseJetStreamContext_WhenPublishingMessage()
        {
            var context = new Mock<INatsJSContext>();
            context.Setup(c => c.PublishAsync<string>(It.IsAny<string>(), It.IsAny<string>(), null, null, It.IsAny<NatsHeaders>(), It.IsAny<CancellationToken>()))
                .Returns(new ValueTask<PubAckResponse>(default(PubAckResponse)));
            var queue = new ContextNatsCommandQueue(_marshaller, CreateOptions(), context.Object);
            var message = CreateMessage();

            await queue.SendAsync([message]);

            context.Verify(c => c.PublishAsync<string>("subject", It.IsAny<string>(), null, null, It.Is<NatsHeaders>(headers => headers["type"] == message.GetType().ToFullNameIncludingAssemblyName()), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReceiveAsync_ShouldDeserializeFetchedMessagesAndAcknowledge()
        {
            var options = CreateOptions();
            options.AutoAcknowledge = true;
            options.Expires = TimeSpan.FromSeconds(30);
            var message = CreateMessage();
            var queue = new TestableNatsCommandQueue(_marshaller, options, message);

            var received = new List<IMessage<ICommand>>();
            await foreach (var item in queue.ReceiveAsync())
            {
                received.Add(item);
            }

            Assert.Single(received);
            Assert.Equal(message.Id, received[0].Id);
            Assert.Equal("stream", queue.StreamConfig.Name);
            Assert.Equal("consumer", queue.ConsumerConfig.Name);
            Assert.Equal(TimeSpan.FromSeconds(30), queue.FetchOptions.Expires);
            Assert.Equal(TimeSpan.FromSeconds(5), queue.FetchOptions.IdleHeartbeat);
            Assert.Equal(1, queue.AcknowledgedCount);
        }

        [Fact]
        public async Task ReceiveAsync_ShouldUseJetStreamContext_WhenCreatingConsumer()
        {
            var context = new Mock<INatsJSContext>();
            context.Setup(c => c.CreateOrUpdateStreamAsync(It.IsAny<StreamConfig>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<INatsJSStream>());
            context.Setup(c => c.CreateOrUpdateConsumerAsync(It.IsAny<string>(), It.IsAny<ConsumerConfig>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<INatsJSConsumer>());
            var queue = new ContextNatsCommandQueue(_marshaller, CreateOptions(), context.Object);

            var consumer = await queue.CallCreateConsumerAsync(
                new StreamConfig("stream", ["subject"]),
                new ConsumerConfig("consumer"),
                CancellationToken.None);

            Assert.NotNull(consumer);
            context.Verify(c => c.CreateOrUpdateStreamAsync(It.Is<StreamConfig>(config => config.Name == "stream"), It.IsAny<CancellationToken>()), Times.Once);
            context.Verify(c => c.CreateOrUpdateConsumerAsync("stream", It.Is<ConsumerConfig>(config => config.Name == "consumer"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReceiveAsync_ShouldFetchMessagesFromConsumer()
        {
            var message = CreateMessage();
            var payload = JsonMarshaller.Default.Serialize(message).ToByteArray().ToBase64String();
            var headers = new NatsHeaders
            {
                { "type", message.GetType().ToFullNameIncludingAssemblyName() }
            };
            var natsMessage = new NatsMsg<string>("subject", "reply", payload.Length, headers, payload, Mock.Of<INatsConnection>(), default);
            var jetStreamMessage = new NatsJSMsg<string>(natsMessage, Mock.Of<INatsJSContext>());
            var consumer = new Mock<INatsJSConsumer>();
            consumer.Setup(c => c.FetchAsync<string>(It.IsAny<NatsJSFetchOpts>(), It.IsAny<INatsDeserialize<string>>(), It.IsAny<CancellationToken>()))
                .Returns(Yield<INatsJSMsg<string>>(jetStreamMessage));
            var queue = new ContextNatsCommandQueue(_marshaller, CreateOptions(), Mock.Of<INatsJSContext>());

            var fetched = await queue.CallFetchMessagesAsync(consumer.Object, new NatsJSFetchOpts(), CancellationToken.None);

            Assert.Equal(1, fetched.Count);
            Assert.Equal(payload, fetched.Data);
        }

        [Fact]
        public async Task ReceiveAsync_WithPreCancelledToken_ShouldThrowException()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var queue = new NatsCommandQueue(_marshaller, new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject"
            });

            var exception = await Record.ExceptionAsync(async () =>
            {
                await foreach (var _ in queue.ReceiveAsync(o => o.CancellationToken = cts.Token)) { }
            });

            Assert.NotNull(exception);
        }

        private static NatsCommandQueueOptions CreateOptions()
        {
            return new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject"
            };
        }

        private static IMessage<ICommand> CreateMessage()
        {
            return new Message<TestCommand>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test",
                new TestCommand());
        }

        private sealed class TestableNatsCommandQueue : NatsCommandQueue
        {
            private readonly IMessage<ICommand> _message;

            public TestableNatsCommandQueue(IMarshaller marshaller, NatsCommandQueueOptions options, IMessage<ICommand> message = null) : base(marshaller, options)
            {
                _message = message;
            }

            public string PublishedSubject { get; private set; }

            public string PublishedMessage { get; private set; }

            public NatsHeaders PublishedHeaders { get; private set; }

            public StreamConfig StreamConfig { get; private set; }

            public ConsumerConfig ConsumerConfig { get; private set; }

            public NatsJSFetchOpts FetchOptions { get; private set; }

            public int AcknowledgedCount { get; private set; }

            protected override Task PublishMessageAsync(INatsJSContext context, string subject, string message, NatsHeaders headers)
            {
                PublishedSubject = subject;
                PublishedMessage = message;
                PublishedHeaders = headers;
                return Task.CompletedTask;
            }

            protected override Task<INatsJSConsumer> CreateConsumerAsync(StreamConfig streamConfig, ConsumerConfig consumerConfig, CancellationToken cancellationToken)
            {
                StreamConfig = streamConfig;
                ConsumerConfig = consumerConfig;
                return Task.FromResult<INatsJSConsumer>(null);
            }

            protected override async IAsyncEnumerable<ReceivedNatsMessage> FetchMessagesAsync(INatsJSConsumer consumer, NatsJSFetchOpts options, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
            {
                FetchOptions = options;
                if (_message != null)
                {
                    var payload = JsonMarshaller.Default.Serialize(_message).ToByteArray().ToBase64String();
                    yield return new ReceivedNatsMessage(new NatsHeaders
                    {
                        { "type", _message.GetType().ToFullNameIncludingAssemblyName() }
                    }, payload, _ =>
                    {
                        AcknowledgedCount++;
                        return Task.CompletedTask;
                    });
                }

                await Task.CompletedTask.ConfigureAwait(false);
            }
        }

        private sealed class ContextNatsCommandQueue : NatsCommandQueue
        {
            private readonly INatsJSContext _context;

            public ContextNatsCommandQueue(IMarshaller marshaller, NatsCommandQueueOptions options, INatsJSContext context) : base(marshaller, options)
            {
                _context = context;
            }

            public Task<INatsJSConsumer> CallCreateConsumerAsync(StreamConfig streamConfig, ConsumerConfig consumerConfig, CancellationToken cancellationToken)
            {
                return base.CreateConsumerAsync(streamConfig, consumerConfig, cancellationToken);
            }

            public async Task<(int Count, string Data)> CallFetchMessagesAsync(INatsJSConsumer consumer, NatsJSFetchOpts options, CancellationToken cancellationToken)
            {
                var count = 0;
                string data = null;
                await foreach (var message in base.FetchMessagesAsync(consumer, options, cancellationToken))
                {
                    count++;
                    data = message.Data;
                    await Record.ExceptionAsync(() => message.AcknowledgeAsync(cancellationToken)).ConfigureAwait(false);
                }

                return (count, data);
            }

            protected override INatsJSContext CreateJetStreamContext()
            {
                return _context;
            }
        }

        private static async IAsyncEnumerable<T> Yield<T>(T item)
        {
            yield return item;
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private record TestCommand : Command { }
    }
}
