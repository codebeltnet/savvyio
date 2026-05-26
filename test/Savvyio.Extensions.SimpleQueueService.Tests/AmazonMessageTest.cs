using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Codebelt.Extensions.Xunit;
using Savvyio.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService
{
    public class AmazonMessageTest : Test
    {
        public AmazonMessageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Should_Set_Protected_Properties_For_Standard_Queue()
        {
            var marshaller = JsonMarshaller.Default;
            var options = CreateOptions("https://sqs.eu-west-1.amazonaws.com/123456789012/orders");

            var sut = new TestAmazonQueue(marshaller, options);

            Assert.Same(marshaller, sut.ExposedMarshaller);
            Assert.Same(options, sut.Options);
            Assert.False(sut.UseFirstInFirstOut);
        }

        [Fact]
        public void Constructor_Should_Detect_First_In_First_Out_Queue()
        {
            var sut = new TestAmazonQueue(JsonMarshaller.Default, CreateOptions("https://sqs.eu-west-1.amazonaws.com/123456789012/orders.fifo"));

            Assert.True(sut.UseFirstInFirstOut);
        }

        private static AmazonMessageOptions CreateOptions(string queueUrl)
        {
            return new AmazonMessageOptions
            {
                Credentials = new AnonymousAWSCredentials(),
                Endpoint = RegionEndpoint.EUWest1,
                SourceQueue = new Uri(queueUrl)
            };
        }

        private sealed class TestAmazonQueue : AmazonQueue<ICommand>
        {
            public TestAmazonQueue(IMarshaller marshaller, AmazonMessageOptions options) : base(marshaller, options)
            {
            }

            public bool UseFirstInFirstOut => base.UseFirstInFirstOut;

            public IMarshaller ExposedMarshaller => base.Marshaller;

            public override Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                return Task.CompletedTask;
            }

            public override async IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                yield break;
            }
        }
    }
}
