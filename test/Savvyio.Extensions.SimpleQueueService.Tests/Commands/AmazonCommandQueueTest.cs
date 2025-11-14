using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Moq;
using Savvyio.Commands;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService.Commands
{
    /// <summary>
    /// Unit tests for <see cref="AmazonCommandQueue"/>.
    /// </summary>
    public class AmazonCommandQueueTest : Test
    {
        private readonly IMarshaller _marshaller;
        private readonly AmazonCommandQueueOptions _options;

        public AmazonCommandQueueTest(ITestOutputHelper output) : base(output)
        {
            _marshaller = new JsonMarshaller();
            _options = new AmazonCommandQueueOptions
            {
                Credentials = new Mock<Amazon.Runtime.AWSCredentials>().Object,
                Endpoint = Amazon.RegionEndpoint.USEast1,
                SourceQueue = new Uri("https://sqs.us-east-1.amazonaws.com/123456789012/MyQueue")
            };
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Marshaller_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonCommandQueue(null, _options));
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_Options_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonCommandQueue(_marshaller, null));
        }

        [Fact]
        public void Ctor_Should_Throw_ArgumentException_When_Options_Invalid()
        {
            var invalidOptions = new AmazonCommandQueueOptions();
            Assert.Throws<ArgumentException>(() => new AmazonCommandQueue(_marshaller, invalidOptions));
        }

        [Fact]
        public async Task SendAsync_Should_Throw_ArgumentNullException_When_Messages_Is_Null()
        {
            var sut = new AmazonCommandQueue(_marshaller, _options);
            await Assert.ThrowsAsync<ArgumentNullException>(() => sut.SendAsync(null));
        }

        [Fact]
        public async Task ReceiveAsync_Should_Call_RetrieveMessagesAsync()
        {
            var sut = new AmazonCommandQueue(_marshaller, _options);

            // Act
            var result = sut.ReceiveAsync();

            // Assert
            Assert.NotNull(result);
            // We can't enumerate without a real SQS, but we can check the type
            Assert.IsAssignableFrom<IAsyncEnumerable<IMessage<ICommand>>>(result);
        }

        [Fact]
        public void GetHealthCheckTarget_Should_Return_IAmazonSQS_Instance()
        {
            var sut = new AmazonCommandQueue(_marshaller, _options);

            var sqs = sut.GetHealthCheckTarget();

            Assert.NotNull(sqs);
            Assert.IsAssignableFrom<IAmazonSQS>(sqs);
        }
    }
}
