using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using NATS.Client.Core;
using Savvyio.EventDriven;
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
            var message = new Message<TestIntegrationEvent>(
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
        public async Task PublishAsync_ShouldPublishSerializedMessage()
        {
            var bus = new TestableNatsEventBus(_marshaller, new NatsEventBusOptions { Subject = "subject" });
            var message = CreateMessage();

            await bus.PublishAsync(message);

            Assert.Equal("subject", bus.PublishedSubject);
            Assert.Equal(message.GetType().ToFullNameIncludingAssemblyName(), bus.PublishedHeaders["type"]);
            Assert.NotNull(bus.PublishedMessage);
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

        [Fact]
        public async Task SubscribeAsync_ShouldDeserializeMessagesAndInvokeHandler()
        {
            var message = CreateMessage();
            var bus = new TestableNatsEventBus(_marshaller, new NatsEventBusOptions { Subject = "subject" }, message);
            IMessage<IIntegrationEvent> received = null;

            await bus.SubscribeAsync((msg, _) =>
            {
                received = msg;
                return Task.CompletedTask;
            });

            Assert.NotNull(received);
            Assert.Equal(message.Id, received.Id);
            Assert.Equal("subject", bus.SubscribedSubject);
        }

        private static IMessage<IIntegrationEvent> CreateMessage()
        {
            return new Message<TestIntegrationEvent>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test.event",
                new TestIntegrationEvent());
        }

        private sealed class TestableNatsEventBus : NatsEventBus
        {
            private readonly IMessage<IIntegrationEvent> _message;

            public TestableNatsEventBus(IMarshaller marshaller, NatsEventBusOptions options, IMessage<IIntegrationEvent> message = null) : base(marshaller, options)
            {
                _message = message;
            }

            public string PublishedSubject { get; private set; }

            public string PublishedMessage { get; private set; }

            public NatsHeaders PublishedHeaders { get; private set; }

            public string SubscribedSubject { get; private set; }

            protected override Task PublishMessageAsync(string subject, string message, NatsHeaders headers, CancellationToken cancellationToken)
            {
                PublishedSubject = subject;
                PublishedMessage = message;
                PublishedHeaders = headers;
                return Task.CompletedTask;
            }

            protected override async IAsyncEnumerable<ReceivedNatsMessage> SubscribeMessagesAsync(string subject, NatsSubOpts options, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
            {
                SubscribedSubject = subject;
                if (_message != null)
                {
                    yield return new ReceivedNatsMessage(new NatsHeaders
                    {
                        { "type", _message.GetType().ToFullNameIncludingAssemblyName() }
                    }, JsonMarshaller.Default.Serialize(_message).ToByteArray().ToBase64String());
                }

                await Task.CompletedTask.ConfigureAwait(false);
            }
        }

        private record TestIntegrationEvent : IntegrationEvent { }
    }
}
