using Cuemon.Extensions.Xunit;
using Savvyio.Messaging;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands.Messaging
{
    public class ReceiveAsyncOptionsTest : Test
    {
        public ReceiveAsyncOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_ShouldHaveDefaultValues()
        {
            var sut = new ReceiveAsyncOptions();

            Assert.Equal(10, sut.MaxNumberOfMessages);
        }

        [Fact]
        public void MaxNumberOfMessages_ShouldClampValue_WhenOutsideRangeOfAllowedLimits()
        {
            var sut = new ReceiveAsyncOptions();

            sut.MaxNumberOfMessages = int.MinValue;
            Assert.Equal(1, sut.MaxNumberOfMessages);

            sut.MaxNumberOfMessages = int.MaxValue;
            Assert.Equal(ushort.MaxValue, sut.MaxNumberOfMessages);
        }
    }
}
