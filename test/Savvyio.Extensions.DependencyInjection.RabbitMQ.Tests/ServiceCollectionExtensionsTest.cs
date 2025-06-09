using System;
using Codebelt.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Extensions.DependencyInjection.RabbitMQ.Assets;
using Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands;
using Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.RabbitMQ.Commands;
using Savvyio.Extensions.RabbitMQ.EventDriven;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Savvyio.Extensions.DependencyInjection.RabbitMQ
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddRabbitMqCommandQueue_ShouldAddDefaultImplementation()
        {
            Action<RabbitMqCommandQueueOptions> rabbitMqSetup = o =>
            { 
                o.QueueName = "urn:queue";
            };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<NewtonsoftJsonMarshaller>();
            sut1.AddRabbitMqCommandQueue(rabbitMqSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<RabbitMqCommandQueue>(sut2.GetRequiredService<IPointToPointChannel<ICommand>>());
            Assert.IsType<RabbitMqCommandQueue>(sut2.GetRequiredService<ISender<ICommand>>());
            Assert.IsType<RabbitMqCommandQueue>(sut2.GetRequiredService<IReceiver<ICommand>>());

            var opt1 = new RabbitMqCommandQueueOptions();
            var opt2 = new RabbitMqCommandQueueOptions();
            var opt3 = new RabbitMqCommandQueueOptions();

            rabbitMqSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<RabbitMqCommandQueueOptions>>().Invoke(opt2);

            Assert.Equivalent(rabbitMqSetup, sut2.GetRequiredService<Action<RabbitMqCommandQueueOptions>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddRabbitMqCommandQueue_ShouldAddDefaultImplementationWithMarker()
        {
            Action<RabbitMqCommandQueueOptions<QueueMarker>> rabbitMqSetup = o =>
            {
                o.QueueName = "urn:queue";
            };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<JsonMarshaller>();
            sut1.AddRabbitMqCommandQueue(rabbitMqSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<RabbitMqCommandQueue<QueueMarker>>(sut2.GetRequiredService<IPointToPointChannel<ICommand, QueueMarker>>());
            Assert.IsType<RabbitMqCommandQueue<QueueMarker>>(sut2.GetRequiredService<ISender<ICommand, QueueMarker>>());
            Assert.IsType<RabbitMqCommandQueue<QueueMarker>>(sut2.GetRequiredService<IReceiver<ICommand, QueueMarker>>());

            var opt1 = new RabbitMqCommandQueueOptions<QueueMarker>();
            var opt2 = new RabbitMqCommandQueueOptions<QueueMarker>();
            var opt3 = new RabbitMqCommandQueueOptions<QueueMarker>();

            rabbitMqSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<RabbitMqCommandQueueOptions<QueueMarker>>>().Invoke(opt2);

            Assert.Equivalent(rabbitMqSetup, sut2.GetRequiredService<Action<RabbitMqCommandQueueOptions<QueueMarker>>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddRabbitMqEventBus_ShouldAddDefaultImplementation()
        {
            Action<RabbitMqEventBusOptions> rabbitMqSetup = o =>
            {
                o.ExchangeName = "urn:bus";
            };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<NewtonsoftJsonMarshaller>();
            sut1.AddRabbitMqEventBus(rabbitMqSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<RabbitMqEventBus>(sut2.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent>>());
            Assert.IsType<RabbitMqEventBus>(sut2.GetRequiredService<IPublisher<IIntegrationEvent>>());
            Assert.IsType<RabbitMqEventBus>(sut2.GetRequiredService<ISubscriber<IIntegrationEvent>>());

            var opt1 = new RabbitMqEventBusOptions();
            var opt2 = new RabbitMqEventBusOptions();
            var opt3 = new RabbitMqEventBusOptions();

            rabbitMqSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<RabbitMqEventBusOptions>>().Invoke(opt2);

            Assert.Equivalent(rabbitMqSetup, sut2.GetRequiredService<Action<RabbitMqEventBusOptions>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }

        [Fact]
        public void AddRabbitMqEventBus_ShouldAddDefaultImplementationWithMarker()
        {
            Action<RabbitMqEventBusOptions<BusMarker>> rabbitMqSetup = o =>
            {
                o.ExchangeName = "urn:bus";
            };

            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<JsonMarshaller>();
            sut1.AddRabbitMqEventBus(rabbitMqSetup);
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<RabbitMqEventBus<BusMarker>>(sut2.GetRequiredService<IPublishSubscribeChannel<IIntegrationEvent, BusMarker>>());
            Assert.IsType<RabbitMqEventBus<BusMarker>>(sut2.GetRequiredService<IPublisher<IIntegrationEvent, BusMarker>>());
            Assert.IsType<RabbitMqEventBus<BusMarker>>(sut2.GetRequiredService<ISubscriber<IIntegrationEvent, BusMarker>>());

            var opt1 = new RabbitMqEventBusOptions<BusMarker>();
            var opt2 = new RabbitMqEventBusOptions<BusMarker>();
            var opt3 = new RabbitMqEventBusOptions<BusMarker>();

            rabbitMqSetup.Invoke(opt1);
            sut2.GetRequiredService<Action<RabbitMqEventBusOptions<BusMarker>>>().Invoke(opt2);

            Assert.Equivalent(rabbitMqSetup, sut2.GetRequiredService<Action<RabbitMqEventBusOptions<BusMarker>>>());
            Assert.Equivalent(opt1, opt2);

            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt1, opt3));
            Assert.Throws<EquivalentException>(() => Assert.Equivalent(opt2, opt3));
        }
    }
}
