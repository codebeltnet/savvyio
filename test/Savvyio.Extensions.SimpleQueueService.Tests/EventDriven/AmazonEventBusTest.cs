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
using Xunit.Abstractions;

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
    }
}
