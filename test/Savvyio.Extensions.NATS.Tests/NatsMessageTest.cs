using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using NATS.Net;
using Savvyio.Extensions.Text.Json;
using Xunit;

namespace Savvyio.Extensions.NATS
{
    public class TestNatsMessage : NatsMessage
    {
        public TestNatsMessage(IMarshaller marshaller, NatsMessageOptions options)
            : base(marshaller, options) { }

        public Task CallOnDisposeManagedResourcesAsync()
        {
            return DisposeAsync().AsTask();
        }

        public NatsClient ExposedNatsClient => NatsClient;
        public IMarshaller ExposedMarshaller => Marshaller;
    }

    public class NatsMessageTest : Test
    {
        public NatsMessageTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var marshaller = JsonMarshaller.Default;
            var options = new NatsMessageOptions { Subject = "foo" };

            var sut = new TestNatsMessage(marshaller, options);

            Assert.NotNull(sut.ExposedNatsClient);
            Assert.Same(marshaller, sut.ExposedMarshaller);
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenMarshallerIsNull()
        {
            var options = new NatsMessageOptions { Subject = "foo" };
            Assert.Throws<ArgumentNullException>(() => new TestNatsMessage(null, options));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestNatsMessage(JsonMarshaller.Default, null));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsInvalid()
        {
            Assert.Throws<ArgumentException>(() => new TestNatsMessage(JsonMarshaller.Default, new NatsMessageOptions()));
        }

        [Fact]
        public void GetHealthCheckTarget_ShouldReturnNatsConnection()
        {
            var sut = new TestNatsMessage(JsonMarshaller.Default, new NatsMessageOptions { Subject = "foo" });

            var connection = sut.GetHealthCheckTarget();

            Assert.NotNull(connection);
            Assert.Same(sut.ExposedNatsClient.Connection, connection);
        }

        [Fact]
        public async Task OnDisposeManagedResourcesAsync_ShouldDisposeNatsClient()
        {
            var sut = new TestNatsMessage(JsonMarshaller.Default, new NatsMessageOptions { Subject = "foo" });

            // NatsClient should not be disposed before
            Assert.False(sut.Disposed);

            await sut.CallOnDisposeManagedResourcesAsync();

            Assert.True(sut.Disposed);
        }
    }
}