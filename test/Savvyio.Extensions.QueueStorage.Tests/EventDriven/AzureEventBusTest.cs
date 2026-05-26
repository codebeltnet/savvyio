using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Messaging.EventGrid;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Reflection;
using Cuemon.Threading;
using Moq;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Savvyio.Extensions.QueueStorage.EventDriven;
using Savvyio.Extensions.Text.Json;
using Xunit;

namespace Savvyio.Extensions.QueueStorage.EventDriven
{
    /// <summary>
    /// Unit tests for <see cref="AzureEventBus"/>.
    /// </summary>
    public class AzureEventBusTest : Test
    {
        public AzureEventBusTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_Should_Throw_When_Options_Invalid()
        {
            var marshaller = new JsonMarshaller();
            var queueOptions = new AzureQueueOptions();

            Assert.Throws<ArgumentException>(() =>
                new AzureEventBus(marshaller, queueOptions, null));
        }

        [Fact]
        public void Ctor_Should_Use_Token_Credential()
        {
            var marshaller = new JsonMarshaller();
            var queueOptions = new AzureQueueOptions
            {
                StorageAccountName = "testaccount",
                QueueName = "testqueue",
                Credential = new Mock<TokenCredential>().Object
            };
            var eventBusOptions = new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic"),
                Credential = new Mock<TokenCredential>().Object
            };

            var bus = new AzureEventBus(marshaller, queueOptions, eventBusOptions);
            Assert.NotNull(bus);
        }

        [Fact]
        public void Ctor_Should_Use_Sas_And_Key_Credentials()
        {
            var marshaller = new JsonMarshaller();
            var queueOptions = new AzureQueueOptions
            {
                StorageAccountName = "testaccount",
                QueueName = "testqueue",
                SasCredential = new AzureSasCredential("sig")
            };
            var eventBusOptions = new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic"),
                KeyCredential = new AzureKeyCredential("key")
            };

            var bus = new AzureEventBus(marshaller, queueOptions, eventBusOptions);
            Assert.NotNull(bus);
        }

        [Fact]
        public async Task PublishAsync_Should_Throw_When_Message_Null()
        {
            var bus = CreateSut();
            await Assert.ThrowsAsync<ArgumentNullException>(() => bus.PublishAsync(null));
        }

        [Fact]
        public async Task PublishAsync_Should_Invoke_EventGridClient()
        {
            var marshaller = new JsonMarshaller();

            var eventGridClient = new Mock<EventGridPublisherClient>();
            eventGridClient
                .Setup(c => c.SendEventAsync(It.IsAny<Azure.Messaging.CloudEvent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<Azure.Response>())
                .Verifiable();

            var bus = CreateSut(marshaller: marshaller);
            SetPrivateClient(bus, eventGridClient.Object);

            var message = new Message<DummyIntegrationEvent>("id", "https://source".ToUri(), "type", new DummyIntegrationEvent(), DateTime.UtcNow);

            await bus.PublishAsync(message);

            eventGridClient.Verify();
        }

        [Fact]
        public async Task PublishAsync_Should_Add_Signature_When_SignedMessage()
        {
            var marshaller = new JsonMarshaller();

            var eventGridClient = new Mock<EventGridPublisherClient>();
            eventGridClient
                .Setup(c => c.SendEventAsync(It.IsAny<Azure.Messaging.CloudEvent>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<Azure.Response>())
                .Verifiable();

            var bus = CreateSut(marshaller: marshaller);
            SetPrivateClient(bus, eventGridClient.Object);

            // Use a real signed message implementation with non-null data
            var signedMessage = new TestSignedMessage(
                id: "id",
                source: "https://source".ToUri(),
                type: "type",
                data: new DummyIntegrationEvent(),
                time: DateTime.UtcNow,
                signature: "signature"
            );

            await bus.PublishAsync(signedMessage);

            eventGridClient.Verify();
        }

        // Dummy event for test
        private record DummyIntegrationEvent : IntegrationEvent
        {
        }

        // Helper test double for ISignedMessage<IIntegrationEvent>
        private record TestSignedMessage : Message<IIntegrationEvent>, ISignedMessage<IIntegrationEvent>
        {
            public TestSignedMessage(string id, Uri source, string type, IIntegrationEvent data, DateTime? time, string signature)
                : base(id, source, type, data, time)
            {
                Signature = signature;
            }

            public string Signature { get; }
        }

        [Fact]
        public async Task SubscribeAsync_Should_Throw_When_Handler_Null()
        {
            var bus = CreateSut();
            await Assert.ThrowsAsync<ArgumentNullException>(() => bus.SubscribeAsync(null));
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Return_Health_Uri()
        {
            var options = new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic")
            };
            var bus = CreateSut(eventBusOptions: options);

            var uri = bus.GetHealthCheckTarget();

            Assert.Equal(new Uri("https://test.topic/api/health"), uri);
        }

        [Fact]
        public async Task SubscribeAsync_ShouldReceiveQueuedCloudEventsAndInvokeHandler()
        {
            var marshaller = new JsonMarshaller();
            var message = new Message<DummyIntegrationEvent>("id", "https://source".ToUri(), "type", new DummyIntegrationEvent(), DateTime.UtcNow);
            var cloudEvent = message.ToCloudEvent();
            cloudEvent.Add(AzureEventBus.CloudEventTypeExtensionAttribute, message.GetType().ToFullNameIncludingAssemblyName());
            var raw = QueuesModelFactory.QueueMessage("message-id", "pop-receipt", marshaller.Serialize(cloudEvent).ToEncodedString().ToByteArray().ToBase64String(), 1, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow);
            var queueClient = new Mock<QueueClient>();
            queueClient.SetupSequence(c => c.ReceiveMessagesAsync(It.IsAny<int?>(), It.IsAny<TimeSpan?>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(new[] { raw }, Mock.Of<Response>()))
                .ReturnsAsync(Response.FromValue(Array.Empty<QueueMessage>(), Mock.Of<Response>()));
            queueClient.Setup(c => c.DeleteMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<Response>());
            var bus = new TestableAzureEventBus(marshaller, queueClient.Object);
            IMessage<IIntegrationEvent> received = null;

            await bus.SubscribeAsync((msg, _) =>
            {
                received = msg;
                return Task.CompletedTask;
            });

            Assert.NotNull(received);
            Assert.Equal(message.Id, received.Id);
        }

        // --- Helpers ---

        private AzureEventBus CreateSut(
            IMarshaller marshaller = null,
            AzureEventBusOptions eventBusOptions = null)
        {
            marshaller ??= new JsonMarshaller();
            var queueOptions = new AzureQueueOptions
            {
                StorageAccountName = "testaccount",
                QueueName = "testqueue",
                Credential = new Mock<TokenCredential>().Object
            };
            eventBusOptions ??= new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic"),
                Credential = new Mock<TokenCredential>().Object
            };

            return new AzureEventBus(marshaller, queueOptions, eventBusOptions);
        }

        private void SetPrivateClient(AzureEventBus bus, EventGridPublisherClient client)
        {
            var field = typeof(AzureEventBus).GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(bus, client);
        }

        private sealed class TestableAzureEventBus : AzureEventBus
        {
            public TestableAzureEventBus(IMarshaller marshaller, QueueClient queueClient) : base(marshaller, new AzureQueueOptions
            {
                StorageAccountName = "testaccount",
                QueueName = "testqueue",
                Credential = new Mock<TokenCredential>().Object
            }, new AzureEventBusOptions
            {
                TopicEndpoint = new Uri("https://test.topic"),
                Credential = new Mock<TokenCredential>().Object
            }, Mock.Of<QueueServiceClient>(), queueClient, Mock.Of<EventGridPublisherClient>())
            {
            }
        }

    }
}