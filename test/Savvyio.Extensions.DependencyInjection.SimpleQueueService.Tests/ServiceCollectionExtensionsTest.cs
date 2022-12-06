using System;
using Amazon;
using Amazon.Runtime;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.Assets;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Extensions.SimpleQueueService.EventDriven;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.SimpleQueueService
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddAmazonCommandQueue_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddAmazonCommandQueue(o =>
            {
                o.Credentials = new AnonymousAWSCredentials();
                o.Endpoint = RegionEndpoint.EUWest1;
                o.SourceQueue = new Uri("urn:queue");
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonCommandQueue>(sut2.GetRequiredService<IPointToPointChannel<ICommand>>());
            Assert.IsType<AmazonCommandQueue>(sut2.GetRequiredService<ISender<ICommand>>());
            Assert.IsType<AmazonCommandQueue>(sut2.GetRequiredService<IReceiver<ICommand>>());
        }

        [Fact]
        public void AddAmazonCommandQueue_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddAmazonCommandQueue<QueueMarker>(o =>
            {
                o.Credentials = new AnonymousAWSCredentials();
                o.Endpoint = RegionEndpoint.EUWest1;
                o.SourceQueue = new Uri("urn:queue");
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonCommandQueue<QueueMarker>>(sut2.GetRequiredService<IPointToPointChannel<ICommand, QueueMarker>>());
            Assert.IsType<AmazonCommandQueue<QueueMarker>>(sut2.GetRequiredService<ISender<ICommand, QueueMarker>>());
            Assert.IsType<AmazonCommandQueue<QueueMarker>>(sut2.GetRequiredService<IReceiver<ICommand, QueueMarker>>());
        }

        [Fact]
        public void AddAmazonEventBus_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddAmazonEventBus(o =>
            {
                o.Credentials = new AnonymousAWSCredentials();
                o.Endpoint = RegionEndpoint.EUWest1;
                o.SourceQueue = new Uri("urn:bus");
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonEventBus>(sut2.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>());
            Assert.IsType<AmazonEventBus>(sut2.GetRequiredService<IPublisher<IIntegrationEvent>>());
            Assert.IsType<AmazonEventBus>(sut2.GetRequiredService<ISubscriber<IIntegrationEvent>>());
        }

        [Fact]
        public void AddAmazonEventBus_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddAmazonEventBus<BusMarker>(o =>
            {
                o.Credentials = new AnonymousAWSCredentials();
                o.Endpoint = RegionEndpoint.EUWest1;
                o.SourceQueue = new Uri("urn:bus");
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonEventBus<BusMarker>>(sut2.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent, BusMarker>>());
            Assert.IsType<AmazonEventBus<BusMarker>>(sut2.GetRequiredService<IPublisher<IIntegrationEvent, BusMarker>>());
            Assert.IsType<AmazonEventBus<BusMarker>>(sut2.GetRequiredService<ISubscriber<IIntegrationEvent, BusMarker>>());
        }
    }
}
