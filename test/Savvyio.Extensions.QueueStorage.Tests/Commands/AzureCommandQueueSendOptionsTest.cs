using System;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.QueueStorage.Commands
{
    public class AzureCommandQueueSendOptionsTest : Test
    {
        public AzureCommandQueueSendOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            var options = new AzureCommandQueueOptions().SendContext;

            Assert.Equal(TimeSpan.Zero, options.VisibilityTimeout);
            Assert.Equal(TimeSpan.FromDays(7), options.TimeToLive);
        }

        [Fact]
        public void VisibilityTimeout_ShouldSetAndGetCorrectly()
        {
            var options = new AzureCommandQueueOptions()
            {
                SendContext = { VisibilityTimeout = TimeSpan.FromMinutes(1) }
            };

            Assert.Equal(TimeSpan.FromMinutes(1), options.SendContext.VisibilityTimeout);
        }

        [Fact]
        public void TimeToLive_ShouldSetAndGetCorrectly()
        {
            var options = new AzureCommandQueueOptions()
            {
                SendContext = { TimeToLive = TimeSpan.FromHours(1) }
            };

            Assert.Equal(TimeSpan.FromHours(1), options.SendContext.TimeToLive);
        }

        [Fact]
        public void VisibilityTimeout_ShouldClampToValidRange()
        {
            var options = new AzureCommandQueueOptions()
            {
                SendContext = { VisibilityTimeout = TimeSpan.FromDays(10) }
            };

            Assert.Equal(AzureCommandQueueOptions.MaxVisibilityTimeout, options.SendContext.VisibilityTimeout);

            options.SendContext.VisibilityTimeout = TimeSpan.FromMilliseconds(-1);

            Assert.Equal(TimeSpan.Zero, options.SendContext.VisibilityTimeout);
        }
    }
}
