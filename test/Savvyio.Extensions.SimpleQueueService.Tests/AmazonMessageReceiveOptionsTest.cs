using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.SimpleQueueService
{
    public class AmazonMessageReceiveOptionsTest : Test
    {
        public AmazonMessageReceiveOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldHaveDefaultValues()
        {
            var sut = new AmazonMessageReceiveOptions();

            Assert.Equal(10, sut.NumberOfMessagesToTakePerRequest);
            Assert.Equal(TimeSpan.FromSeconds(AmazonMessageOptions.MaxPollingWaitTimeInSeconds), sut.PollingTimeout);
            Assert.True(sut.RemoveProcessedMessages);
            Assert.False(sut.UseApproximateNumberOfMessages);
        }

        [Fact]
        public void NumberOfMessagesToTakePerRequest_ShouldClampValue_WhenOutsideRangeOfAllowedLimits()
        {
            var sut = new AmazonMessageReceiveOptions();

            sut.NumberOfMessagesToTakePerRequest = int.MinValue;
            Assert.Equal(1, sut.NumberOfMessagesToTakePerRequest);

            sut.NumberOfMessagesToTakePerRequest = int.MaxValue;
            Assert.Equal(AmazonMessageOptions.MaxNumberOfMessages, sut.NumberOfMessagesToTakePerRequest);
        }

        [Fact]
        public void PollingTimeout_ShouldClampValue_WhenOutsideRangeOfAllowedLimits()
        {
            var sut = new AmazonMessageReceiveOptions();

            sut.PollingTimeout = TimeSpan.MinValue;
            Assert.Equal(TimeSpan.Zero, sut.PollingTimeout);

            sut.PollingTimeout = TimeSpan.MaxValue;
            Assert.Equal(TimeSpan.FromSeconds(AmazonMessageOptions.MaxPollingWaitTimeInSeconds), sut.PollingTimeout);
        }

        [Fact]
        public void VisibilityTimeout_ShouldClampValue_WhenOutsideRangeOfAllowedLimits()
        {
            var sut = new AmazonMessageReceiveOptions();

            sut.VisibilityTimeout = TimeSpan.MinValue;
            Assert.Equal(TimeSpan.Zero, sut.VisibilityTimeout);

            sut.VisibilityTimeout = TimeSpan.FromSeconds(AmazonMessageOptions.MaxVisibilityTimeoutInSeconds + 1);
            Assert.Equal(TimeSpan.FromSeconds(AmazonMessageOptions.MaxVisibilityTimeoutInSeconds), sut.VisibilityTimeout);
        }
    }
}
