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
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Threading;
using Moq;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging.CloudEvents;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;
using Savvyio.Extensions.QueueStorage.EventDriven;
using Savvyio.Extensions.Text.Json;
using Xunit;
using Xunit.Abstractions;

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
        public void Ctor_Should_Use_Correct_Credential()
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

            var message = new Message<IIntegrationEvent>("id", "https://source".ToUri(), "type", new DummyIntegrationEvent(), DateTime.UtcNow);

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
        private class DummyIntegrationEvent : IIntegrationEvent
        {
            public IMetadataDictionary Metadata { get; }
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

    }
}