using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Codebelt.Extensions.Xunit;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using Moq;
using Savvyio.EventDriven;
using Savvyio.Extensions.SimpleQueueService.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService.EventDriven
{
    /// <summary>
    /// Unit tests for <see cref="AmazonEventBus"/>.
    /// </summary>
    public class AmazonEventBusTest : Test
    {
        public AmazonEventBusTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_Should_ThrowArgumentNullException_When_MarshallerIsNull()
        {
            var options = new AmazonEventBusOptions
            {
                Credentials = new BasicAWSCredentials("key", "secret"),
                Endpoint = RegionEndpoint.EUWest1
            };

            Assert.Throws<ArgumentNullException>(() => new AmazonEventBus(null, options));
        }

        [Fact]
        public void Ctor_Should_ThrowArgumentNullException_When_OptionsIsNull()
        {
            var marshaller = new JsonMarshaller();

            Assert.Throws<ArgumentNullException>(() => new AmazonEventBus(marshaller, null));
        }

        [Fact]
        public void Ctor_Should_ThrowArgumentException_When_OptionsInvalid()
        {
            var marshaller = new JsonMarshaller();
            var options = new AmazonEventBusOptions(); // Missing required properties

            Assert.Throws<ArgumentException>(() => new AmazonEventBus(marshaller, options));
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Return_IAmazonSimpleNotificationService_Instance()
        {
            var marshaller = new JsonMarshaller();
            var options = new AmazonEventBusOptions
            {
                Credentials = new BasicAWSCredentials("key", "secret"),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/MyQueue")
            };

            var sut = new AmazonEventBus(marshaller, options);

            var sns = sut.GetHealthCheckTarget();

            Assert.NotNull(sns);
            Assert.IsAssignableFrom<IAmazonSimpleNotificationService>(sns);
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Use_Configured_Client_When_Available()
        {
            var marshaller = new JsonMarshaller();
            var options = new AmazonEventBusOptions
            {
                Credentials = new BasicAWSCredentials("key", "secret"),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/MyQueue")
            };
            options.ConfigureClient(config =>
            {
                config.ServiceURL = "http://localhost:4566";
                config.AuthenticationRegion = RegionEndpoint.EUWest1.SystemName;
            });

            var sut = new AmazonEventBus(marshaller, options);

            var sns = sut.GetHealthCheckTarget();

            Assert.Equal("http://localhost:4566/", sns.Config.ServiceURL);
            Assert.Equal(RegionEndpoint.EUWest1.SystemName, sns.Config.AuthenticationRegion);
        }

        [Fact]
        public async Task PublishAsync_ShouldSendSerializedEvent()
        {
            var sns = new Mock<IAmazonSimpleNotificationService>();
            PublishRequest request = null;
            sns.Setup(s => s.PublishAsync(It.IsAny<PublishRequest>(), It.IsAny<CancellationToken>()))
                .Callback<PublishRequest, CancellationToken>((r, _) => request = r)
                .ReturnsAsync(new PublishResponse());
            var bus = new TestableAmazonEventBus(new JsonMarshaller(), CreateOptions("https://sqs.eu-west-1.amazonaws.com/123456789012/MyQueue.fifo"), sns.Object);
            var message = CreateMessage();

            await bus.PublishAsync(message);

            Assert.NotNull(request);
            Assert.Equal(message.Source, request.TopicArn);
            Assert.Equal(message.Source, request.MessageGroupId);
            Assert.Equal(message.Id, request.MessageDeduplicationId);
            Assert.Equal(message.GetType().ToFullNameIncludingAssemblyName(), request.MessageAttributes["type"].StringValue);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldReceiveMessagesAndInvokeHandler()
        {
            var message = CreateMessage();
            var sqsMessage = new Message
            {
                Body = new JsonMarshaller().Serialize(message).ToEncodedString(),
                MessageId = "message-id",
                ReceiptHandle = "receipt-handle",
                MessageAttributes = new Dictionary<string, Amazon.SQS.Model.MessageAttributeValue>
                {
                    ["type"] = new Amazon.SQS.Model.MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = message.GetType().ToFullNameIncludingAssemblyName()
                    }
                }
            };
            var sqs = new Mock<IAmazonSQS>();
            sqs.SetupSequence(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReceiveMessageResponse { Messages = [sqsMessage] })
                .ReturnsAsync(new ReceiveMessageResponse { Messages = [] });
            sqs.Setup(s => s.DeleteMessageBatchAsync(It.IsAny<DeleteMessageBatchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new DeleteMessageBatchResponse());
            var bus = new TestableAmazonEventBus(new JsonMarshaller(), CreateOptions("https://sqs.eu-west-1.amazonaws.com/123456789012/MyQueue"), Mock.Of<IAmazonSimpleNotificationService>(), sqs.Object);
            IMessage<IIntegrationEvent> received = null;

            await bus.SubscribeAsync((msg, _) =>
            {
                received = msg;
                return Task.CompletedTask;
            });

            Assert.NotNull(received);
            Assert.Equal(message.Id, received.Id);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldSwallowCancellation_WhenThrowIfCancellationWasRequestedIsFalse()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            var sqs = new Mock<IAmazonSQS>();
            sqs.Setup(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException(cts.Token));
            var bus = new TestableAmazonEventBus(new JsonMarshaller(), CreateOptions("https://sqs.eu-west-1.amazonaws.com/123456789012/MyQueue"), Mock.Of<IAmazonSimpleNotificationService>(), sqs.Object);

            await bus.SubscribeAsync((_, _) => Task.CompletedTask, o => o.CancellationToken = cts.Token);

            sqs.Verify(s => s.ReceiveMessageAsync(It.IsAny<ReceiveMessageRequest>(), cts.Token), Times.Once);
        }

        private static AmazonEventBusOptions CreateOptions(string queueUrl)
        {
            return new AmazonEventBusOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri(queueUrl)
            };
        }

        private static IMessage<IIntegrationEvent> CreateMessage()
        {
            return new Message<DummyIntegrationEvent>("id", new Uri("arn:aws:sns:eu-west-1:123456789012:topic.fifo"), "type", new DummyIntegrationEvent(), DateTime.UtcNow);
        }

        private sealed record DummyIntegrationEvent : IntegrationEvent
        {
        }

        private sealed class TestableAmazonEventBus : AmazonEventBus
        {
            private readonly IAmazonSimpleNotificationService _sns;
            private readonly IAmazonSQS _sqs;

            public TestableAmazonEventBus(IMarshaller marshaller, AmazonEventBusOptions options, IAmazonSimpleNotificationService sns, IAmazonSQS sqs = null) : base(marshaller, options)
            {
                _sns = sns;
                _sqs = sqs;
            }

            protected override IAmazonSimpleNotificationService CreateSimpleNotificationServiceClient()
            {
                return _sns;
            }

            protected override IAmazonSQS CreateSimpleQueueServiceClient()
            {
                return _sqs ?? base.CreateSimpleQueueServiceClient();
            }
        }
    }
}
