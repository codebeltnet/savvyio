using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.QueueStorage
{
    public class AzureQueueReceiveOptionsTest : Test
    {
        public AzureQueueReceiveOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var options = new AzureQueueOptions().ReceiveContext;

            Assert.Equal(10, options.NumberOfMessagesToTakePerRequest);
            Assert.Equal(TimeSpan.FromSeconds(30), options.VisibilityTimeout);
        }

        [Fact]
        public void NumberOfMessagesToTakePerRequest_ShouldSetAndGetCorrectly()
        {
            var options = new AzureQueueOptions()
            {
                ReceiveContext = { NumberOfMessagesToTakePerRequest = 5 }
            };

            Assert.Equal(5, options.ReceiveContext.NumberOfMessagesToTakePerRequest);
        }

        [Fact]
        public void NumberOfMessagesToTakePerRequest_ShouldClampToValidRange()
        {
            var options = new AzureQueueOptions()
            {
                ReceiveContext = { NumberOfMessagesToTakePerRequest = 50 }
            };

            Assert.Equal(AzureQueueOptions.MaxNumberOfMessages, options.ReceiveContext.NumberOfMessagesToTakePerRequest);

            options.ReceiveContext.NumberOfMessagesToTakePerRequest = -1;

            Assert.Equal(1, options.ReceiveContext.NumberOfMessagesToTakePerRequest);
        }

        [Fact]
        public void VisibilityTimeout_ShouldSetAndGetCorrectly()
        {
            var options = new AzureQueueOptions()
            {
                ReceiveContext = { VisibilityTimeout = TimeSpan.FromMinutes(1) }
            };

            Assert.Equal(TimeSpan.FromMinutes(1), options.ReceiveContext.VisibilityTimeout);
        }

        [Fact]
        public void VisibilityTimeout_ShouldClampToValidRange()
        {
            var options = new AzureQueueOptions()
            {
                ReceiveContext = { VisibilityTimeout = TimeSpan.FromDays(10) }
            };

            Assert.Equal(AzureQueueOptions.MaxVisibilityTimeout, options.ReceiveContext.VisibilityTimeout);

            options.ReceiveContext.VisibilityTimeout = TimeSpan.FromMilliseconds(500);

            Assert.Equal(TimeSpan.FromSeconds(1), options.ReceiveContext.VisibilityTimeout);
        }
    }
}
