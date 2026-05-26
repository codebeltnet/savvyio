using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Moq;
using Savvyio.Commands;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
    /// <summary>
    /// Unit tests for <see cref="AmazonCommandQueue"/>.
    /// </summary>
    public class AmazonCommandQueueTest : Test
    {
        private readonly IMarshaller _marshaller;
        private readonly AmazonCommandQueueOptions _options;

        public AmazonCommandQueueTest(ITestOutputHelper output) : base(output)
        {
            _marshaller = new JsonMarshaller();
            _options = new AmazonCommandQueueOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = Amazon.RegionEndpoint.USEast1,
                SourceQueue = new Uri("https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue")
            };
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Marshaller_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonCommandQueue(null, _options));
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Options_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonCommandQueue(_marshaller, null));
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentException_When_Options_Invalid()
        {
            var invalidOptions = new AmazonCommandQueueOptions();
            Assert.Throws<ArgumentException>(() => new AmazonCommandQueue(_marshaller, invalidOptions));
        }

        [Fact]
        public async Task SendAsync_Should_Throw_ArgumentNullException_When_Messages_Is_Null()
        {
            var sut = new AmazonCommandQueue(_marshaller, _options);
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.SendAsync(null));
        }

        [Fact]
        public async Task ReceiveAsync_Should_Call_RetrieveMessagesAsync()
        {
            var sut = new AmazonCommandQueue(_marshaller, _options);

            // Act
            var result = sut.ReceiveAsync();

            // Assert
            Assert.NotNull(result);
            // We can't enumerate without a real SQS, but we can check the type
            Assert.IsAssignableFrom<IAsyncEnumerable<IMessage<ICommand>>>(result);
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Return_IAmazonSQS_Instance()
        {
            var sut = new AmazonCommandQueue(_marshaller, _options);

            var sqs = sut.GetHealthCheckTarget();

            Assert.NotNull(sqs);
            Assert.IsAssignableFrom<IAmazonSQS>(sqs);
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Use_Configured_Client_When_Available()
        {
            var options = new AmazonCommandQueueOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = Amazon.RegionEndpoint.USEast1,
                SourceQueue = new Uri("https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue")
            };
            options.ConfigureClient(config =>
            {
                config.ServiceURL = "http://localhost:4566";
                config.AuthenticationRegion = Amazon.RegionEndpoint.USEast1.SystemName;
            });

            var sut = new AmazonCommandQueue(_marshaller, options);

            var sqs = sut.GetHealthCheckTarget();

            Assert.Equal("http://localhost:4566/", sqs.Config.ServiceURL);
            Assert.Equal(Amazon.RegionEndpoint.USEast1.SystemName, sqs.Config.AuthenticationRegion);
        }

        [Fact]
        public async Task SendAsync_ShouldSendMessagesInBatches()
        {
            var sqs = new Mock<IAmazonSQS>();
            var requests = new List<SendMessageBatchRequest>();
            sqs.Setup(s => s.SendMessageBatchAsync(It.IsAny<SendMessageBatchRequest>(), It.IsAny<CancellationToken>()))
                .Callback<SendMessageBatchRequest, CancellationToken>((request, _) => requests.Add(request))
                .ReturnsAsync(new SendMessageBatchResponse());
            var queue = new TestableAmazonCommandQueue(_marshaller, _options, sqs.Object);
            var messages = Enumerable.Range(0, 11).Select(_ => CreateMessage()).ToArray();

            await queue.SendAsync(messages);

            Assert.Equal(2, requests.Count);
            Assert.Equal(10, requests[0].Entries.Count);
            Assert.Single(requests[1].Entries);
            Assert.All(requests.SelectMany(request => request.Entries), entry =>
            {
                Assert.NotNull(entry.MessageBody);
                Assert.True(entry.MessageAttributes.ContainsKey("type"));
            });
        }

        [Fact]
        public async Task ReceiveAsync_ShouldDeserializeMessagesAndDeleteAcknowledgedMessages()
        {
            var message = CreateMessage();
            var awsMessage = new Message
            {
                Body = _marshaller.Serialize(message).ToEncodedString(),
                MessageId = "message-id",
                ReceiptHandle = "receipt-handle",
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    ["type"] = new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = message.GetType().ToFullNameIncludingAssemblyName()
                    }
                }
            };
            var sqs = new Mock<IAmazonSQS>();
            sqs.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReceiveMessageResponse
                {
                    Messages = [awsMessage]
                });
            DeleteMessageBatchRequest deleteRequest = null;
            sqs.Setup(s => s.DeleteMessageBatchAsync(It.IsAny<DeleteMessageBatchRequest>(), It.IsAny<CancellationToken>()))
                .Callback<DeleteMessageBatchRequest, CancellationToken>((request, _) => deleteRequest = request)
                .ReturnsAsync(new DeleteMessageBatchResponse());
            var queue = new TestableAmazonCommandQueue(_marshaller, _options, sqs.Object);

            var received = new List<IMessage<ICommand>>();
            await foreach (var item in queue.ReceiveAsync())
            {
                received.Add(item);
            }

            Assert.Single(received);
            Assert.Equal(message.Id, received[0].Id);
            Assert.NotNull(deleteRequest);
            Assert.Equal("message-id", deleteRequest.Entries[0].Id);
            Assert.Equal("receipt-handle", deleteRequest.Entries[0].ReceiptHandle);
        }

        [Fact]
        public async Task ReceiveAsync_ShouldUseApproximateNumberOfMessages_WhenConfigured()
        {
            var message = CreateMessage();
            var awsMessage = new Message
            {
                Body = _marshaller.Serialize(message).ToEncodedString(),
                MessageId = "message-id",
                ReceiptHandle = "receipt-handle",
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    ["type"] = new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = message.GetType().ToFullNameIncludingAssemblyName()
                    }
                }
            };
            var options = new AmazonCommandQueueOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = Amazon.RegionEndpoint.USEast1,
                SourceQueue = new Uri("https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue")
            };
            options.ReceiveContext.UseApproximateNumberOfMessages = true;
            var sqs = new Mock<IAmazonSQS>();
            sqs.Setup(s => s.GetQueueAttributesAsync(It.IsAny<GetQueueAttributesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetQueueAttributesResponse
                {
                    Attributes = new Dictionary<string, string>
                    {
                        ["ApproximateNumberOfMessages"] = "1"
                    }
                });
            sqs.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReceiveMessageResponse
                {
                    Messages = [awsMessage]
                });
            sqs.Setup(s => s.DeleteMessageBatchAsync(It.IsAny<DeleteMessageBatchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteMessageBatchResponse());
            var queue = new TestableAmazonCommandQueue(_marshaller, options, sqs.Object);

            var received = new List<IMessage<ICommand>>();
            await foreach (var item in queue.ReceiveAsync())
            {
                received.Add(item);
            }

            Assert.Single(received);
            sqs.Verify(s => s.GetQueueAttributesAsync(It.Is<GetQueueAttributesRequest>(request => request.QueueUrl == options.SourceQueue.OriginalString), It.IsAny<CancellationToken>()), Times.Once);
        }

        private static IMessage<ICommand> CreateMessage()
        {
            return new Message<TestCommand>(
                Guid.NewGuid().ToString("N"),
                new Uri("urn:test"),
                "test",
                new TestCommand());
        }

        private sealed class TestableAmazonCommandQueue : AmazonCommandQueue
        {
            private readonly IAmazonSQS _sqs;

            public TestableAmazonCommandQueue(IMarshaller marshaller, AmazonCommandQueueOptions options, IAmazonSQS sqs) : base(marshaller, options)
            {
                _sqs = sqs;
            }

            protected override IAmazonSQS CreateSimpleQueueServiceClient()
            {
                return _sqs;
            }
        }

        private record TestCommand : Command { }
    }
}
