using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Messaging
{
    public class MessageBatchOptionsTest : Test
    {
        public MessageBatchOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MessageBatchOptions_ShouldHaveDefaultValues()
        {
            var sut = new MessageBatchOptions();

            Assert.Equal(10, sut.MaxNumberOfMessages);
        }

        [Fact]
        public void MessageBatchOptions_ShouldClampValue_WhenOutsideRangeOfAllowedLimits()
        {
            var sut = new MessageBatchOptions();

            sut.MaxNumberOfMessages = int.MinValue;
            Assert.Equal(1, sut.MaxNumberOfMessages);

            sut.MaxNumberOfMessages = int.MaxValue;
            Assert.Equal(ushort.MaxValue, sut.MaxNumberOfMessages);
        }
    }
}
