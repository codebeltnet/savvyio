using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Moq;
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

        [Fact]
        public async Task SendMessageAsync_ShouldSendFormattedMessages()
        {
            var queueClient = new Mock<QueueClient>();
            var sent = new List<string>();
            queueClient.Setup(c => c.SendMessageAsync(It.IsAny<string>(), It.IsAny<TimeSpan?>(), It.IsAny<TimeSpan?>(), default))
                .Callback<string, TimeSpan?, TimeSpan?, System.Threading.CancellationToken>((message, _, _, _) => sent.Add(message))
                .ReturnsAsync(Response.FromValue(QueuesModelFactory.SendReceipt("id", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), "receipt", DateTimeOffset.UtcNow), Mock.Of<Response>()));
            var queue = new TestAzureQueue(_marshaller, _options, queueClient.Object);

            await queue.SendAsync([CreateMessage()]);

            Assert.Single(sent);
            Assert.Contains(".", sent[0], StringComparison.Ordinal);
        }

        [Fact]
        public async Task ReceiveMessagesAsync_ShouldYieldMessagesAndDeleteOnAcknowledge()
        {
            var message = CreateMessage();
            var encodedType = message.GetType().ToFullNameIncludingAssemblyName().ToByteArray().ToBase64String();
            var encodedMessage = _marshaller.Serialize(message).ToByteArray().ToBase64String();
            var raw = QueuesModelFactory.QueueMessage("message-id", "pop-receipt", $"{encodedType}.{encodedMessage}", 1, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow);
            var queueClient = new Mock<QueueClient>();
            queueClient.Setup(c => c.ReceiveMessagesAsync(It.IsAny<int?>(), It.IsAny<TimeSpan?>(), default))
                .ReturnsAsync(Response.FromValue(new[] { raw }, Mock.Of<Response>()));
            string deletedMessageId = null;
            queueClient.Setup(c => c.DeleteMessageAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .Callback<string, string, System.Threading.CancellationToken>((messageId, _, _) => deletedMessageId = messageId)
                .ReturnsAsync(Mock.Of<Response>());
            var queue = new TestAzureQueue(_marshaller, _options, queueClient.Object);

            var received = new List<IMessage<ICommand>>();
            await foreach (var item in queue.ReceiveAsync())
            {
                received.Add(item);
                await item.AcknowledgeAsync();
            }

            Assert.Single(received);
            Assert.Equal(message.Id, received[0].Id);
            Assert.Equal("message-id", deletedMessageId);
        }

        private static IMessage<ICommand> CreateMessage()
        {
            return new Message<TestCommand>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test",
                new TestCommand());
        }

        private sealed class TestAzureQueue : AzureQueue<ICommand>
        {
            public TestAzureQueue(IMarshaller marshaller, AzureQueueOptions options, QueueClient client) : base(marshaller, options, Mock.Of<QueueServiceClient>(), client)
            {
            }

            public Task SendAsync(IEnumerable<IMessage<ICommand>> messages)
            {
                return SendMessageAsync(messages);
            }

            public IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync()
            {
                return ReceiveMessagesAsync();
            }
        }

        private record TestCommand : Command { }
    }
}
