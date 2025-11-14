using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Codebelt.Extensions.Xunit;
using Moq;
using Moq.Protected;
using Savvyio.Commands;
using Savvyio.Extensions.QueueStorage.Commands;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    /// <summary>
    /// Unit tests for <see cref="AzureCommandQueue"/>.
    /// </summary>
    public class AzureCommandQueueTest : Test
    {
        private readonly Mock<IMarshaller> _marshallerMock;
        private readonly AzureQueueOptions _options;

        public AzureCommandQueueTest(ITestOutputHelper output) : base(output)
        {
            _marshallerMock = new Mock<IMarshaller>(MockBehavior.Strict);
            _options = new AzureQueueOptions { QueueName = "test-queue", StorageAccountName = "test-account" };
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenMarshallerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AzureCommandQueue(null, _options));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentNullException_WhenOptionsIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AzureCommandQueue(_marshallerMock.Object, null));
        }

        [Fact]
        public void Constructor_ShouldCreateInstance_WhenArgumentsAreValid()
        {
            var queue = new AzureCommandQueue(_marshallerMock.Object, _options);
            Assert.NotNull(queue);
        }

        [Fact]
        public async Task SendAsync_ShouldReturnTask()
        {
            // Arrange
            var queue = new AzureCommandQueue(_marshallerMock.Object, _options);
            var messages = new List<IMessage<ICommand>>();

            // Act
            var task = queue.SendAsync(messages);

            // Assert
            Assert.NotNull(task);
            await task; // Ensure it completes without exception
        }

        [Fact]
        public void ReceiveAsync_ShouldReturnAsyncEnumerable()
        {
            // Arrange
            var queue = new AzureCommandQueue(_marshallerMock.Object, _options);

            // Act
            var result = queue.ReceiveAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IAsyncEnumerable<IMessage<ICommand>>>(result);
        }

        [Fact]
        public void GetHealthCheckTarget_ShouldReturnQueueServiceClient()
        {
            // Arrange
            var queue = new AzureCommandQueue(_marshallerMock.Object, _options);

            // Act
            var result = queue.GetHealthCheckTarget();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<QueueServiceClient>(result);
        }
    }
}
