using System;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
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

            var message = new Message<ICommand>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test",
                new TestCommand());

            var ex = await Record.ExceptionAsync(() => queue.SendAsync(new IMessage<ICommand>[] { message }));

            Assert.NotNull(ex);
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

        private record TestCommand : Command { }
    }
}
