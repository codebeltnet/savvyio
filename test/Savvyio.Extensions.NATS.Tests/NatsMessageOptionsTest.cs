    using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.NATS
{
    public class NatsMessageOptionsTest : Test
    {
        public NatsMessageOptionsTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Constructor_ShouldSetDefaults()
        {
            var options = new NatsMessageOptions();
            Assert.NotNull(options.NatsUrl);
            Assert.Equal("nats://127.0.0.1:4222", options.NatsUrl.OriginalString);
            Assert.Null(options.Subject);
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenNatsUrlIsNull()
        {
            var options = new NatsMessageOptions { NatsUrl = null, Subject = "foo" };
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenSubjectIsNullOrWhitespace()
        {
            var options = new NatsMessageOptions { Subject = null };
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.Subject = "";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.Subject = "   ";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_ShouldNotThrow_WhenValid()
        {
            var options = new NatsMessageOptions { Subject = "foo" };
            var ex = Record.Exception(() => options.ValidateOptions());
            Assert.Null(ex);
        }
    }
}
