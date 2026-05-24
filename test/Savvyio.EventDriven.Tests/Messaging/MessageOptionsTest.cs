using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Messaging
{
    public class MessageOptionsTest : Test
    {
        public MessageOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenMessageIdIsNull()
        {
            var options = new MessageOptions { MessageId = null };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenTimeIsNotUtc()
        {
            var options = new MessageOptions { Time = DateTime.Now };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldSucceed_WhenValid()
        {
            var options = new MessageOptions { MessageId = "abc", Time = DateTime.UtcNow };

            options.ValidateOptions();
        }
    }
}
