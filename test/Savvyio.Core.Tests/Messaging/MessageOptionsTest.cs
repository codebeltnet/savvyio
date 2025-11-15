using System;
using Cuemon;
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
        public void MessageOptions_ShouldHaveDefaultValues()
        {
            var sut = new MessageOptions();

            Assert.NotNull(sut.MessageId);
            Assert.Null(sut.Time);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void MessageOptions_ThrowsInvalidOperationException_WhenMessageIdIsNullOrEmptyOrConsistOfWhiteSpaceOnly(string messageId)
        {
            var sut1 = new MessageOptions
            {
                MessageId = messageId
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(MessageOptions.MessageId)}.IsNullOrWhiteSpace()')", sut2.Message);
            Assert.Equal($"{nameof(MessageOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }

        [Fact]
        public void MessageOptions_ThrowsInvalidOperationException_WhenTimeIsLocal()
        {
            var sut1 = new MessageOptions
            {
                Time = DateTime.Now
            };
            var sut2 = Assert.Throws<InvalidOperationException>(() => sut1.ValidateOptions());
            var sut3 = Assert.Throws<ArgumentException>(() => Validator.ThrowIfInvalidOptions(sut1));

            Assert.Equal($"Operation is not valid due to the current state of the object. (Expression '{nameof(MessageOptions.Time)}.HasValue && {nameof(MessageOptions.Time)}.Value.Kind != DateTimeKind.Utc')", sut2.Message);
            Assert.Equal($"{nameof(MessageOptions)} are not in a valid state. (Parameter '{nameof(sut1)}')", sut3.Message);
            Assert.IsType<InvalidOperationException>(sut3.InnerException);
        }
    }
}
