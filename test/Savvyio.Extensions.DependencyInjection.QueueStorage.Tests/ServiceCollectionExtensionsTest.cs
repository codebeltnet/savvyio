using System;
using Azure;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.QueueStorage;
using Savvyio.Extensions.DependencyInjection.QueueStorage.Assets;
using DICommands = Savvyio.Extensions.DependencyInjection.QueueStorage.Commands;
using DIEvent = Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.QueueStorage;
using CoreCommands = Savvyio.Extensions.QueueStorage.Commands;
using CoreEvent = Savvyio.Extensions.QueueStorage.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Savvyio.Extensions.DependencyInjection.QueueStorage
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddAzureCommandQueue_ShouldAddDefaultImplementation()
        {
            Action<AzureQueueOptions> azureSetup = o =>
            {
                o.StorageAccountName = "account";
                o.QueueName = "queue";
                o.SasCredential = new AzureSasCredential("sas");
            };

            var services = new ServiceCollection();
            services.AddMarshaller<NewtonsoftJsonMarshaller>();
            services.AddAzureCommandQueue(azureSetup);
            var provider = services.BuildServiceProvider();

            Assert.IsType<CoreCommands.AzureCommandQueue>(provider.GetRequiredService<IPointToPointChannel<ICommand>>());
            Assert.IsType<CoreCommands.AzureCommandQueue>(provider.GetRequiredService<ISender<ICommand>>());
            Assert.IsType<CoreCommands.AzureCommandQueue>(provider.GetRequiredService<IReceiver<ICommand>>());

            var opt1 = new AzureQueueOptions();
            var opt2 = new AzureQueueOptions();
            var opt3 = new AzureQueueOptions();

            azureSetup(opt1);
            provider.GetRequiredService<Action<AzureQueueOptions>>()(opt2);

            Assert.Equivalent(azureSetup, provider.GetRequiredService<Action<AzureQueueOptions>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddAzureCommandQueue_ShouldAddDefaultImplementationWithMarker()
        {
            Action<AzureQueueOptions<QueueMarker>> azureSetup = o =>
            {
                o.StorageAccountName = "account";
                o.QueueName = "queue";
                o.SasCredential = new AzureSasCredential("sas");
            };

            var services = new ServiceCollection();
            services.AddMarshaller<JsonMarshaller>();
            services.AddAzureCommandQueue(azureSetup);
            var provider = services.BuildServiceProvider();

            Assert.IsType<DICommands.AzureCommandQueue<QueueMarker>>(provider.GetRequiredService<IPointToPointChannel<ICommand, QueueMarker>>());
            Assert.IsType<DICommands.AzureCommandQueue<QueueMarker>>(provider.GetRequiredService<ISender<ICommand, QueueMarker>>());
            Assert.IsType<DICommands.AzureCommandQueue<QueueMarker>>(provider.GetRequiredService<IReceiver<ICommand, QueueMarker>>());

            var opt1 = new AzureQueueOptions<QueueMarker>();
            var opt2 = new AzureQueueOptions<QueueMarker>();
            var opt3 = new AzureQueueOptions<QueueMarker>();

            azureSetup(opt1);
            provider.GetRequiredService<Action<AzureQueueOptions<QueueMarker>>>()(opt2);

            Assert.Equivalent(azureSetup, provider.GetRequiredService<Action<AzureQueueOptions<QueueMarker>>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddAzureEventBus_ShouldAddDefaultImplementation()
        {
            Action<AzureQueueOptions> azureQueueSetup = o =>
            {
                o.StorageAccountName = "account";
                o.QueueName = "queue";
                o.SasCredential = new AzureSasCredential("sas");
            };
            Action<CoreEvent.AzureEventBusOptions> azureEventBusSetup = o =>
            {
                o.TopicEndpoint = new Uri("https://example.com");
                o.SasCredential = new AzureSasCredential("sas");
            };

            var services = new ServiceCollection();
            services.AddMarshaller<NewtonsoftJsonMarshaller>();
            services.AddAzureEventBus(azureQueueSetup, azureEventBusSetup);
            var provider = services.BuildServiceProvider();

            Assert.IsType<CoreEvent.AzureEventBus>(provider.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>());
            Assert.IsType<CoreEvent.AzureEventBus>(provider.GetRequiredService<IPublisher<IIntegrationEvent>>());
            Assert.IsType<CoreEvent.AzureEventBus>(provider.GetRequiredService<ISubscriber<IIntegrationEvent>>());

            var q1 = new AzureQueueOptions();
            var q2 = new AzureQueueOptions();
            var q3 = new AzureQueueOptions();
            var b1 = new CoreEvent.AzureEventBusOptions();
            var b2 = new CoreEvent.AzureEventBusOptions();
            var b3 = new CoreEvent.AzureEventBusOptions();

            azureQueueSetup(q1);
            provider.GetRequiredService<Action<AzureQueueOptions>>()(q2);
            azureEventBusSetup(b1);
            provider.GetRequiredService<Action<CoreEvent.AzureEventBusOptions>>()(b2);

            Assert.Equivalent(azureQueueSetup, provider.GetRequiredService<Action<AzureQueueOptions>>());
            Assert.Equivalent(azureEventBusSetup, provider.GetRequiredService<Action<CoreEvent.AzureEventBusOptions>>());
            Assert.Equivalent(q1, q2);
            Assert.Equivalent(b1, b2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(q1, q3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(q2, q3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(b1, b3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(b2, b3));
        }

        [Fact]
        public void AddAzureEventBus_ShouldAddDefaultImplementationWithMarker()
        {
            Action<AzureQueueOptions<BusMarker>> azureQueueSetup = o =>
            {
                o.StorageAccountName = "account";
                o.QueueName = "queue";
                o.SasCredential = new AzureSasCredential("sas");
            };
            Action<DIEvent.AzureEventBusOptions<BusMarker>> azureEventBusSetup = o =>
            {
                o.TopicEndpoint = new Uri("https://example.com");
                o.SasCredential = new AzureSasCredential("sas");
            };

            var services = new ServiceCollection();
            services.AddMarshaller<JsonMarshaller>();
            services.AddAzureEventBus(azureQueueSetup, azureEventBusSetup);
            var provider = services.BuildServiceProvider();

            Assert.IsType<DIEvent.AzureEventBus<BusMarker>>(provider.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent, BusMarker>>());
            Assert.IsType<DIEvent.AzureEventBus<BusMarker>>(provider.GetRequiredService<IPublisher<IIntegrationEvent, BusMarker>>());
            Assert.IsType<DIEvent.AzureEventBus<BusMarker>>(provider.GetRequiredService<ISubscriber<IIntegrationEvent, BusMarker>>());

            var q1 = new AzureQueueOptions<BusMarker>();
            var q2 = new AzureQueueOptions<BusMarker>();
            var q3 = new AzureQueueOptions<BusMarker>();
            var b1 = new DIEvent.AzureEventBusOptions<BusMarker>();
            var b2 = new DIEvent.AzureEventBusOptions<BusMarker>();
            var b3 = new DIEvent.AzureEventBusOptions<BusMarker>();

            azureQueueSetup(q1);
            provider.GetRequiredService<Action<AzureQueueOptions<BusMarker>>>()(q2);
            azureEventBusSetup(b1);
            provider.GetRequiredService<Action<DIEvent.AzureEventBusOptions<BusMarker>>>()(b2);

            Assert.Equivalent(azureQueueSetup, provider.GetRequiredService<Action<AzureQueueOptions<BusMarker>>>());
            Assert.Equivalent(azureEventBusSetup, provider.GetRequiredService<Action<DIEvent.AzureEventBusOptions<BusMarker>>>());
            Assert.Equivalent(q1, q2);
            Assert.Equivalent(b1, b2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(q1, q3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(q2, q3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(b1, b3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(b2, b3));
        }
    }
}
