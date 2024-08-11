using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    public class AzureCommandQueueReceiveOptionsTest : Test
    {
        public AzureCommandQueueReceiveOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var options = new AzureCommandQueueOptions().ReceiveContext;

            Assert.Equal(10, options.NumberOfMessagesToTakePerRequest);
            Assert.Equal(TimeSpan.FromSeconds(30), options.VisibilityTimeout);
        }

        [Fact]
        public void NumberOfMessagesToTakePerRequest_ShouldSetAndGetCorrectly()
        {
            var options = new AzureCommandQueueOptions()
            {
                ReceiveContext = { NumberOfMessagesToTakePerRequest = 5 }
            };

            Assert.Equal(5, options.ReceiveContext.NumberOfMessagesToTakePerRequest);
        }

        [Fact]
        public void NumberOfMessagesToTakePerRequest_ShouldClampToValidRange()
        {
            var options = new AzureCommandQueueOptions()
            {
                ReceiveContext = { NumberOfMessagesToTakePerRequest = 50 }
            };

            Assert.Equal(AzureCommandQueueOptions.MaxNumberOfMessages, options.ReceiveContext.NumberOfMessagesToTakePerRequest);

            options.ReceiveContext.NumberOfMessagesToTakePerRequest = -1;

            Assert.Equal(1, options.ReceiveContext.NumberOfMessagesToTakePerRequest);
        }

        [Fact]
        public void VisibilityTimeout_ShouldSetAndGetCorrectly()
        {
            var options = new AzureCommandQueueOptions()
            {
                ReceiveContext = { VisibilityTimeout = TimeSpan.FromMinutes(1) }
            };

            Assert.Equal(TimeSpan.FromMinutes(1), options.ReceiveContext.VisibilityTimeout);
        }

        [Fact]
        public void VisibilityTimeout_ShouldClampToValidRange()
        {
            var options = new AzureCommandQueueOptions()
            {
                ReceiveContext = { VisibilityTimeout = TimeSpan.FromDays(10) }
            };

            Assert.Equal(AzureCommandQueueOptions.MaxVisibilityTimeout, options.ReceiveContext.VisibilityTimeout);

            options.ReceiveContext.VisibilityTimeout = TimeSpan.FromMilliseconds(500);

            Assert.Equal(TimeSpan.FromSeconds(1), options.ReceiveContext.VisibilityTimeout);
        }
    }
}
