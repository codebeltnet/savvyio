using System;
using System.IO;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Xunit;
using Savvyio.Extensions.NATS;
using NATS.Client.Core;
using System.Threading;
using NATS.Net;

namespace Savvyio.Extensions.NATS
{
    // Minimal fake marshaller for testing
    public class FakeMarshaller : IMarshaller
    {
        public Stream Serialize<TValue>(TValue value) => new MemoryStream();
        public Stream Serialize(object value, Type inputType) => new MemoryStream();
        public TValue Deserialize<TValue>(Stream data) => default;
        public object Deserialize(Stream data, Type returnType) => null;
    }

    // Minimal concrete NatsMessage for testing
    public class TestNatsMessage : NatsMessage
    {
        public TestNatsMessage(IMarshaller marshaller, NatsMessageOptions options)
            : base(marshaller, options) { }

        // Expose protected OnDisposeManagedResourcesAsync for testing
        public async Task CallOnDisposeManagedResourcesAsync()
        {
            await DisposeAsync();
        }

        // Expose protected NatsClient for assertions
        public NatsClient ExposedNatsClient => NatsClient;
        public IMarshaller ExposedMarshaller => Marshaller;
    }

    public class NatsMessageTest : Test
    {
        public NatsMessageTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            var marshaller = new FakeMarshaller();
            var options = new NatsMessageOptions { Subject = "foo" };

            var sut = new TestNatsMessage(marshaller, options);

            Assert.NotNull(sut.ExposedNatsClient);
            Assert.Equal(marshaller, sut.ExposedMarshaller);
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
            var marshaller = new FakeMarshaller();
            Assert.Throws<ArgumentNullException>(() => new TestNatsMessage(marshaller, null));
        }

        [Fact]
        public void Constructor_ShouldThrow_WhenOptionsInvalid()
        {
            var marshaller = new FakeMarshaller();
            var options = new NatsMessageOptions { Subject = null }; // Invalid: Subject is null
            Assert.Throws<ArgumentException>(() => new TestNatsMessage(marshaller, options));
        }

        [Fact]
        public void GetHealthCheckTarget_ShouldReturnNatsConnection()
        {
            var marshaller = new FakeMarshaller();
            var options = new NatsMessageOptions { Subject = "foo" };
            var sut = new TestNatsMessage(marshaller, options);

            var connection = sut.GetHealthCheckTarget();

            Assert.NotNull(connection);
            Assert.Same(sut.ExposedNatsClient.Connection, connection);
        }

        [Fact]
        public async Task OnDisposeManagedResourcesAsync_ShouldDisposeNatsClient()
        {
            var marshaller = new FakeMarshaller();
            var options = new NatsMessageOptions { Subject = "foo" };
            var sut = new TestNatsMessage(marshaller, options);

            // NatsClient should not be disposed before
            Assert.False(sut.Disposed);

            await sut.CallOnDisposeManagedResourcesAsync();

            Assert.True(sut.Disposed);
        }
    }
}