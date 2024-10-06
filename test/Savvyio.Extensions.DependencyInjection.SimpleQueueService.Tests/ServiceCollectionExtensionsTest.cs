using System;
using Amazon;
using Amazon.Runtime;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.Assets;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Extensions.SimpleQueueService.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

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
	        Action<AmazonCommandQueueOptions> awsSetup = o =>
	        {
		        o.Credentials = new AnonymousAWSCredentials();
		        o.Endpoint = RegionEndpoint.EUWest1;
		        o.SourceQueue = new Uri("urn:queue");
	        };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<NewtonsoftJsonMarshaller>();
            sut1.AddAmazonCommandQueue(awsSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonCommandQueue>(sut2.GetRequiredService<IPointToPointChannel<ICommand>>());
            Assert.IsType<AmazonCommandQueue>(sut2.GetRequiredService<ISender<ICommand>>());
            Assert.IsType<AmazonCommandQueue>(sut2.GetRequiredService<IReceiver<ICommand>>());

            var opt1 = new AmazonCommandQueueOptions();
            var opt2 = new AmazonCommandQueueOptions();
            var opt3 = new AmazonCommandQueueOptions();
            
            awsSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<AmazonCommandQueueOptions>>().Invoke(opt2);

            Assert.Equivalent(awsSetup, sut2.GetRequiredService<Action<AmazonCommandQueueOptions>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddAmazonCommandQueue_ShouldAddDefaultImplementationWithMarker()
        {
	        Action<AmazonCommandQueueOptions<QueueMarker>> awsSetup = o =>
	        {
		        o.Credentials = new AnonymousAWSCredentials();
		        o.Endpoint = RegionEndpoint.EUWest1;
		        o.SourceQueue = new Uri("urn:queue");
	        };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<JsonMarshaller>();
            sut1.AddAmazonCommandQueue(awsSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonCommandQueue<QueueMarker>>(sut2.GetRequiredService<IPointToPointChannel<ICommand, QueueMarker>>());
            Assert.IsType<AmazonCommandQueue<QueueMarker>>(sut2.GetRequiredService<ISender<ICommand, QueueMarker>>());
            Assert.IsType<AmazonCommandQueue<QueueMarker>>(sut2.GetRequiredService<IReceiver<ICommand, QueueMarker>>());

            var opt1 = new AmazonCommandQueueOptions<QueueMarker>();
            var opt2 = new AmazonCommandQueueOptions<QueueMarker>();
            var opt3 = new AmazonCommandQueueOptions<QueueMarker>();
            
            awsSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<AmazonCommandQueueOptions<QueueMarker>>>().Invoke(opt2);

            Assert.Equivalent(awsSetup, sut2.GetRequiredService<Action<AmazonCommandQueueOptions<QueueMarker>>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddAmazonEventBus_ShouldAddDefaultImplementation()
        {
	        Action<AmazonEventBusOptions> awsSetup = o =>
	        {
		        o.Credentials = new AnonymousAWSCredentials();
		        o.Endpoint = RegionEndpoint.EUWest1;
		        o.SourceQueue = new Uri("urn:bus");
	        };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<NewtonsoftJsonMarshaller>();
            sut1.AddAmazonEventBus(awsSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonEventBus>(sut2.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>());
            Assert.IsType<AmazonEventBus>(sut2.GetRequiredService<IPublisher<IIntegrationEvent>>());
            Assert.IsType<AmazonEventBus>(sut2.GetRequiredService<ISubscriber<IIntegrationEvent>>());

            var opt1 = new AmazonEventBusOptions();
            var opt2 = new AmazonEventBusOptions();
            var opt3 = new AmazonEventBusOptions();
            
            awsSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<AmazonEventBusOptions>>().Invoke(opt2);

            Assert.Equivalent(awsSetup, sut2.GetRequiredService<Action<AmazonEventBusOptions>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddAmazonEventBus_ShouldAddDefaultImplementationWithMarker()
        {
	        Action<AmazonEventBusOptions<BusMarker>> awsSetup = o =>
	        {
		        o.Credentials = new AnonymousAWSCredentials();
		        o.Endpoint = RegionEndpoint.EUWest1;
		        o.SourceQueue = new Uri("urn:bus");
	        };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<JsonMarshaller>();
            sut1.AddAmazonEventBus(awsSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<AmazonEventBus<BusMarker>>(sut2.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent, BusMarker>>());
            Assert.IsType<AmazonEventBus<BusMarker>>(sut2.GetRequiredService<IPublisher<IIntegrationEvent, BusMarker>>());
            Assert.IsType<AmazonEventBus<BusMarker>>(sut2.GetRequiredService<ISubscriber<IIntegrationEvent, BusMarker>>());

            var opt1 = new AmazonEventBusOptions<BusMarker>();
            var opt2 = new AmazonEventBusOptions<BusMarker>();
            var opt3 = new AmazonEventBusOptions<BusMarker>();
            
            awsSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<AmazonEventBusOptions<BusMarker>>>().Invoke(opt2);

            Assert.Equivalent(awsSetup, sut2.GetRequiredService<Action<AmazonEventBusOptions<BusMarker>>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }
    }
}
