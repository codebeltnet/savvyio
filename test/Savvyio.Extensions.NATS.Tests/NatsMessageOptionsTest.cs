using System;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.NATS
{
    public class NatsMessageOptionsTest : Test
    {
        public NatsMessageOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Sets_Default_Values()
        {
            var options = new NatsMessageOptions();

            Assert.Equal(new Uri("nats://127.0.0.1:4222"), options.NatsUrl);
            Assert.Null(options.Subject);
        }

        [Fact]
        public void ValidateOptions_Throws_When_NatsUrl_Is_Null()
        {
            var options = new NatsMessageOptions
            {
                NatsUrl = null,
                Subject = "subject"
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ValidateOptions_Throws_When_Subject_Is_Null_Or_Whitespace(string subject)
        {
            var options = new NatsMessageOptions
            {
                NatsUrl = new Uri("nats://localhost:4222"),
                Subject = subject
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Does_Not_Throw_When_Valid()
        {
            var options = new NatsMessageOptions
            {
                NatsUrl = new Uri("nats://localhost:4222"),
                Subject = "valid-subject"
            };

            var ex = Record.Exception(() => options.ValidateOptions());
            Assert.Null(ex);
        }
    }
}
