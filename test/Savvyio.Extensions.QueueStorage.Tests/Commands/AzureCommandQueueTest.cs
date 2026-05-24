using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Codebelt.Extensions.Xunit;
using Savvyio.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    public class AzureCommandQueueTest : Test
    {
        private readonly IMarshaller _marshaller;
        private readonly AzureQueueOptions _options;

        public AzureCommandQueueTest(ITestOutputHelper output) : base(output)
        {
            _marshaller = JsonMarshaller.Default;
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
            Assert.Throws<ArgumentNullException>(() => new AzureCommandQueue(_marshaller, null));
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenOptionsAreInvalid()
        {
            Assert.Throws<ArgumentException>(() => new AzureCommandQueue(_marshaller, new AzureQueueOptions { Credential = null }));
        }

        [Fact]
        public void Constructor_ShouldCreateInstance_WhenArgumentsAreValid()
        {
            var queue = new AzureCommandQueue(_marshaller, _options);
            Assert.NotNull(queue);
        }

        [Fact]
        public async Task SendAsync_ShouldThrowArgumentNullException_WhenMessagesAreNull()
        {
            var queue = new AzureCommandQueue(_marshaller, _options);

            await Assert.ThrowsAsync<ArgumentNullException>(() => queue.SendAsync(null));
        }

        [Fact]
        public void ReceiveAsync_ShouldReturnAsyncEnumerable()
        {
            var queue = new AzureCommandQueue(_marshaller, _options);

            var result = queue.ReceiveAsync();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<IAsyncEnumerable<IMessage<ICommand>>>(result);
        }

        [Fact]
        public void GetHealthCheckTarget_ShouldReturnQueueServiceClient_ForConnectionString()
        {
            var queue = new AzureCommandQueue(_marshaller, new AzureQueueOptions
            {
                ConnectionString = "UseDevelopmentStorage=true",
                QueueName = "testqueue"
            });

            var result = queue.GetHealthCheckTarget();

            Assert.NotNull(result);
            Assert.IsType<QueueServiceClient>(result);
            Assert.StartsWith("http://127.0.0.1:10001/devstoreaccount1", result.Uri.AbsoluteUri, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetHealthCheckTarget_ShouldReturnQueueServiceClient_ForStorageSharedKeyCredential()
        {
            var queue = new AzureCommandQueue(_marshaller, new AzureQueueOptions
            {
                StorageAccountName = "testaccount",
                QueueName = "testqueue",
                StorageKeyCredential = new StorageSharedKeyCredential("testaccount", Convert.ToBase64String(new byte[32]))
            });

            var result = queue.GetHealthCheckTarget();

            Assert.Equal(new Uri("https://testaccount.queue.core.windows.net/testqueue"), result.Uri);
        }
    }
}
