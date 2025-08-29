using System;
using Codebelt.Extensions.Xunit;
using Savvyio.EventDriven;
using Savvyio.Extensions.NATS.Commands;
using Savvyio.Extensions.NATS.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

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
    }
}
