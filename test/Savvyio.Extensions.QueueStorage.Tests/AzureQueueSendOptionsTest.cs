using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.QueueStorage
{
    public class AzureQueueSendOptionsTest : Test
    {
        public AzureQueueSendOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var options = new AzureQueueOptions().SendContext;

            Assert.Equal(TimeSpan.Zero, options.VisibilityTimeout);
            Assert.Equal(TimeSpan.FromDays(7), options.TimeToLive);
        }

        [Fact]
        public void VisibilityTimeout_ShouldSetAndGetCorrectly()
        {
            var options = new AzureQueueOptions()
            {
                SendContext = { VisibilityTimeout = TimeSpan.FromMinutes(1) }
            };

            Assert.Equal(TimeSpan.FromMinutes(1), options.SendContext.VisibilityTimeout);
        }

        [Fact]
        public void TimeToLive_ShouldSetAndGetCorrectly()
        {
            var options = new AzureQueueOptions()
            {
                SendContext = { TimeToLive = TimeSpan.FromHours(1) }
            };

            Assert.Equal(TimeSpan.FromHours(1), options.SendContext.TimeToLive);
        }

        [Fact]
        public void VisibilityTimeout_ShouldClampToValidRange()
        {
            var options = new AzureQueueOptions()
            {
                SendContext = { VisibilityTimeout = TimeSpan.FromDays(10) }
            };

            Assert.Equal(AzureQueueOptions.MaxVisibilityTimeout, options.SendContext.VisibilityTimeout);

            options.SendContext.VisibilityTimeout = TimeSpan.FromMilliseconds(-1);

            Assert.Equal(TimeSpan.Zero, options.SendContext.VisibilityTimeout);
        }
    }
}
