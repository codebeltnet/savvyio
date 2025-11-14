using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.NATS.Assets;
using Savvyio.Extensions.DependencyInjection.NATS.Commands;
using Savvyio.Extensions.DependencyInjection.NATS.EventDriven;
using Savvyio.Extensions.NATS.Commands;
using Savvyio.Extensions.NATS.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection.NATS
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddNatsCommandQueue_ShouldRegisterNatsCommandQueueAndConfigureOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            bool natsSetupCalled = false;

            // Act
            services.AddMarshaller<JsonMarshaller>();
            services.AddNatsCommandQueue(options =>
            {
                natsSetupCalled = true;
                options.StreamName = "stream";
                options.ConsumerName = "consumer";
                options.Subject = "subject";
            });

            // Assert
            Assert.True(natsSetupCalled);
            var provider = services.BuildServiceProvider();
            var queue = provider.GetService<NatsCommandQueue>();
            
            
            Assert.IsType<NatsCommandQueue>(provider.GetRequiredService<IPointToPointChannel<ICommand>>());
            Assert.IsType<NatsCommandQueue>(provider.GetRequiredService<ISender<ICommand>>());
            Assert.IsType<NatsCommandQueue>(provider.GetRequiredService<IReceiver<ICommand>>());
            
            Assert.NotNull(queue);
        }

        [Fact]
        public void AddNatsCommandQueue_Generic_ShouldRegisterNatsCommandQueueAndConfigureOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            bool natsSetupCalled = false;

            // Act
            services.AddMarshaller<JsonMarshaller>();
            services.AddNatsCommandQueue<QueueMarker>(options =>
            {
                natsSetupCalled = true;
                options.StreamName = "stream";
                options.ConsumerName = "consumer";
                options.Subject = "subject";
            });

            // Assert
            Assert.True(natsSetupCalled);
            var provider = services.BuildServiceProvider();
            var queue = provider.GetService<NatsCommandQueue<QueueMarker>>();

            Assert.IsType<NatsCommandQueue<QueueMarker>>(provider.GetRequiredService<IPointToPointChannel<ICommand, QueueMarker>>());
            Assert.IsType<NatsCommandQueue<QueueMarker>>(provider.GetRequiredService<ISender<ICommand, QueueMarker>>());
            Assert.IsType<NatsCommandQueue<QueueMarker>>(provider.GetRequiredService<IReceiver<ICommand, QueueMarker>>());

            Assert.NotNull(queue);
        }

        [Fact]
        public void AddNatsEventBus_ShouldRegisterNatsEventBusAndConfigureOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            bool natsSetupCalled = false;

            // Act
            services.AddMarshaller<JsonMarshaller>();
            services.AddNatsEventBus(options =>
            {
                natsSetupCalled = true;
                options.Subject = "subject";
            });

            // Assert
            Assert.True(natsSetupCalled);
            var provider = services.BuildServiceProvider();
            var bus = provider.GetService<NatsEventBus>();

            Assert.IsType<NatsEventBus>(provider.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>());
            Assert.IsType<NatsEventBus>(provider.GetRequiredService<IPublisher<IIntegrationEvent>>());
            Assert.IsType<NatsEventBus>(provider.GetRequiredService<ISubscriber<IIntegrationEvent>>());

            Assert.NotNull(bus);
        }

        [Fact]
        public void AddNatsEventBus_Generic_ShouldRegisterNatsEventBusAndConfigureOptions()
        {
            // Arrange
            var services = new ServiceCollection();
            bool natsSetupCalled = false;

            // Act
            services.AddMarshaller<JsonMarshaller>();
            services.AddNatsEventBus<BusMarker>(options =>
            {
                natsSetupCalled = true;
                options.Subject = "subject";
            });

            // Assert
            Assert.True(natsSetupCalled);
            var provider = services.BuildServiceProvider();
            var bus = provider.GetService<NatsEventBus<BusMarker>>();

            Assert.IsType<NatsEventBus<BusMarker>>(provider.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent, BusMarker>>());
            Assert.IsType<NatsEventBus<BusMarker>>(provider.GetRequiredService<IPublisher<IIntegrationEvent, BusMarker>>());
            Assert.IsType<NatsEventBus<BusMarker>>(provider.GetRequiredService<ISubscriber<IIntegrationEvent, BusMarker>>());

            Assert.NotNull(bus);
        }
    }
}
