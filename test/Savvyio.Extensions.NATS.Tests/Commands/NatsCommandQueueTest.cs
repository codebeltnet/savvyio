using System;
using Codebelt.Extensions.Xunit;
using Savvyio.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

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
    }
}
