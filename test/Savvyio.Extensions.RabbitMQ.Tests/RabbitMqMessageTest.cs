using Codebelt.Extensions.Xunit;
using Moq;
using RabbitMQ.Client;
using Savvyio.Extensions.Text.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Savvyio.Extensions.RabbitMQ
{
    public class RabbitMqMessageTest : Test
    {
        public RabbitMqMessageTest(ITestOutputHelper output) : base(output)
        {
        }

        private sealed class TestRabbitMqMessage : RabbitMqMessage
        {
            public TestRabbitMqMessage(IMarshaller marshaller, RabbitMqMessageOptions options) : base(marshaller, options)
            {
            }

            public Task CallEnsureConnectivityAsync(CancellationToken ct = default) => EnsureConnectivityAsync(ct);

            public ValueTask CallOnDisposeManagedResourcesAsync() => OnDisposeManagedResourcesAsync();

            public void SetConnection(IConnection connection)
            {
                typeof(RabbitMqMessage)
                    .GetProperty("RabbitMqConnection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .SetValue(this, connection);
            }

            public void SetChannel(IChannel channel)
            {
                typeof(RabbitMqMessage)
                    .GetProperty("RabbitMqChannel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .SetValue(this, channel);
            }
        }

        [Fact]
        public void Constructor_Throws_WhenMarshallerIsNull()
        {
            var options = new RabbitMqMessageOptions();
            Assert.Throws<ArgumentNullException>(() => new TestRabbitMqMessage(null, options));
        }

        [Fact]
        public void Constructor_Throws_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new TestRabbitMqMessage(JsonMarshaller.Default, null));
        }

        [Fact]
        public void Constructor_SetsProperties()
        {
            var options = new RabbitMqMessageOptions { AmqpUrl = new Uri("amqp://localhost:5672") };
            var sut = new TestRabbitMqMessage(JsonMarshaller.Default, options);

            Assert.NotNull(sut);
            Assert.NotNull(typeof(RabbitMqMessage).GetProperty("Marshaller", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(sut));
            Assert.NotNull(typeof(RabbitMqMessage).GetProperty("RabbitMqFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(sut));
        }

        [Fact]
        public async Task EnsureConnectivityAsync_InitializesConnectionAndChannel_Once()
        {
            var options = new RabbitMqMessageOptions { AmqpUrl = new Uri("amqp://localhost:5672") };
            var connectionMock = new Mock<IConnection>();
            var channelMock = new Mock<IChannel>();
            connectionMock.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions?>(), It.IsAny<CancellationToken>())).ReturnsAsync(channelMock.Object);

            var factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(connectionMock.Object);

            var sut = new TestRabbitMqMessage(JsonMarshaller.Default, options);
            typeof(RabbitMqMessage)
                .GetProperty("RabbitMqFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(sut, factoryMock.Object);

            await sut.CallEnsureConnectivityAsync();
            await sut.CallEnsureConnectivityAsync();

            Assert.Same(connectionMock.Object, typeof(RabbitMqMessage).GetProperty("RabbitMqConnection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(sut));
            Assert.Same(channelMock.Object, typeof(RabbitMqMessage).GetProperty("RabbitMqChannel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(sut));
            factoryMock.Verify(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task EnsureConnectivityAsync_IsThreadSafe_WhenCalledConcurrently()
        {
            var options = new RabbitMqMessageOptions { AmqpUrl = new Uri("amqp://localhost:5672") };
            var connectionMock = new Mock<IConnection>();
            var channelMock = new Mock<IChannel>();
            connectionMock.Setup(c => c.CreateChannelAsync(It.IsAny<CreateChannelOptions?>(), It.IsAny<CancellationToken>())).ReturnsAsync(channelMock.Object);

            var factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(connectionMock.Object);

            var sut = new TestRabbitMqMessage(JsonMarshaller.Default, options);
            typeof(RabbitMqMessage)
                .GetProperty("RabbitMqFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(sut, factoryMock.Object);

            await Task.WhenAll(sut.CallEnsureConnectivityAsync(), sut.CallEnsureConnectivityAsync());

            factoryMock.Verify(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task OnDisposeManagedResourcesAsync_DisposesChannelAndConnection()
        {
            var channelMock = new Mock<IChannel>();
            channelMock.Setup(c => c.DisposeAsync()).Returns(ValueTask.CompletedTask).Verifiable();

            var connectionMock = new Mock<IConnection>();
            connectionMock.Setup(c => c.DisposeAsync()).Returns(ValueTask.CompletedTask).Verifiable();

            var sut = new TestRabbitMqMessage(JsonMarshaller.Default, new RabbitMqMessageOptions());
            sut.SetChannel(channelMock.Object);
            sut.SetConnection(connectionMock.Object);

            await sut.CallOnDisposeManagedResourcesAsync();

            channelMock.Verify(c => c.DisposeAsync(), Times.Once);
            connectionMock.Verify(c => c.DisposeAsync(), Times.Once);
        }

        [Fact]
        public async Task GetHealthCheckTargetAsync_ReturnsExistingConnection_IfInitialized()
        {
            var connectionMock = new Mock<IConnection>().Object;
            var sut = new TestRabbitMqMessage(JsonMarshaller.Default, new RabbitMqMessageOptions());
            sut.SetConnection(connectionMock);

            var result = await sut.GetHealthCheckTargetAsync();

            Assert.Same(connectionMock, result);
        }

        [Fact]
        public async Task GetHealthCheckTargetAsync_CreatesConnection_IfNotInitialized()
        {
            var connectionMock = new Mock<IConnection>().Object;
            var factoryMock = new Mock<IConnectionFactory>();
            factoryMock.Setup(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(connectionMock);

            var sut = new TestRabbitMqMessage(JsonMarshaller.Default, new RabbitMqMessageOptions());
            typeof(RabbitMqMessage)
                .GetProperty("RabbitMqFactory", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(sut, factoryMock.Object);

            var result = await sut.GetHealthCheckTargetAsync();

            Assert.Same(connectionMock, result);
            factoryMock.Verify(f => f.CreateConnectionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
