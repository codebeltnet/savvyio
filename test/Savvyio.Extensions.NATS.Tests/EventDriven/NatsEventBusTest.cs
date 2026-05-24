using System;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Savvyio.EventDriven;
using Savvyio.Extensions.NATS.Commands;
using Savvyio.Extensions.NATS.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.NATS.EventDriven
{
    public class NatsEventBusTest : Test
    {
        private IMarshaller _marshaller = JsonMarshaller.Default;
        private NatsEventBusOptions _options = new ();

        public NatsEventBusTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldInitialize_WithValidOptions()
        {
            var options = new NatsEventBusOptions
            {
                Subject = "subject"
            };

            var bus = new NatsEventBus(_marshaller, options);

            Assert.NotNull(bus);
            Assert.IsAssignableFrom<IPublishSubscribeChannel<IIntegrationEvent>>(bus);
            Assert.IsAssignableFrom<IPublisher<IIntegrationEvent>>(bus);
            Assert.IsAssignableFrom<ISubscriber<IIntegrationEvent>>(bus);
        }


        [Fact]
        public void Constructor_ShouldThrow_WhenMarshallerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NatsEventBus(null, _options));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NatsEventBus(_marshaller, null));
        }

        
        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsAreInvalid()
        {
            Assert.Throws<ArgumentException>(() => new NatsEventBus(_marshaller, _options));
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Return_Connection()
        {
            var bus = new NatsEventBus(_marshaller, new NatsEventBusOptions
            {
                Subject = "subject"
            });

            Assert.Same(bus.GetHealthCheckTarget(), bus.GetHealthCheckTarget());
            Assert.NotNull(bus.GetHealthCheckTarget());
        }

        [Fact]
        public async Task PublishAsync_WithPreCancelledToken_ShouldThrowOperationCanceledException()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var bus = new NatsEventBus(_marshaller, new NatsEventBusOptions { Subject = "subject" });
            var message = new Message<IIntegrationEvent>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test.event",
                new TestIntegrationEvent());

            var ex = await Record.ExceptionAsync(
                () => bus.PublishAsync(message, o => o.CancellationToken = cts.Token));

            Assert.NotNull(ex);
            Assert.IsAssignableFrom<OperationCanceledException>(ex);
        }

        [Fact]
        public async Task SubscribeAsync_WithPreCancelledToken_ShouldThrowOperationCanceledException()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var bus = new NatsEventBus(_marshaller, new NatsEventBusOptions { Subject = "subject" });

            var ex = await Record.ExceptionAsync(
                () => bus.SubscribeAsync(
                    (msg, ct) => Task.CompletedTask,
                    o => o.CancellationToken = cts.Token));

            Assert.NotNull(ex);
            Assert.IsAssignableFrom<OperationCanceledException>(ex);
        }

        private record TestIntegrationEvent : IntegrationEvent { }
    }
}
