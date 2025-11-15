using Codebelt.Extensions.Xunit;
using System;
using Xunit;

namespace Savvyio.Extensions.NATS.Commands
{
    public class NatsCommandQueueOptionsTest : Test
    {
        public NatsCommandQueueOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Sets_Default_Values()
        {
            var options = new NatsCommandQueueOptions();

            Assert.Equal(25, options.MaxMessages);
            Assert.Equal(TimeSpan.Zero, options.Heartbeat);
            Assert.Equal(TimeSpan.Zero, options.Expires);
            Assert.Null(options.StreamName);
            Assert.Null(options.ConsumerName);
            Assert.False(options.AutoAcknowledge);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(25, 25)]
        [InlineData(32767, 32767)]
        [InlineData(40000, 32767)]
        public void MaxMessages_Clamps_Value(int input, int expected)
        {
            var options = new NatsCommandQueueOptions
            {
                MaxMessages = input
            };

            Assert.Equal(expected, options.MaxMessages);
        }

        [Fact]
        public void Expires_Sets_Heartbeat_When_Expires_AtLeast_30Seconds()
        {
            var options = new NatsCommandQueueOptions();

            options.Expires = TimeSpan.FromSeconds(29);
            Assert.Equal(TimeSpan.FromSeconds(29), options.Expires);
            Assert.Equal(TimeSpan.Zero, options.Heartbeat);

            options.Expires = TimeSpan.FromSeconds(30);
            Assert.Equal(TimeSpan.FromSeconds(30), options.Expires);
            Assert.Equal(TimeSpan.FromSeconds(5), options.Heartbeat);

            options.Expires = TimeSpan.FromSeconds(60);
            Assert.Equal(TimeSpan.FromSeconds(60), options.Expires);
            Assert.Equal(TimeSpan.FromSeconds(5), options.Heartbeat);
        }

        [Fact]
        public void Heartbeat_Can_Be_Set_Manually()
        {
            var options = new NatsCommandQueueOptions
            {
                Heartbeat = TimeSpan.FromSeconds(10)
            };

            Assert.Equal(TimeSpan.FromSeconds(10), options.Heartbeat);
        }

        [Fact]
        public void Heartbeat_Can_Be_Overridden_After_Expires()
        {
            var options = new NatsCommandQueueOptions();

            options.Expires = TimeSpan.FromSeconds(60);
            Assert.Equal(TimeSpan.FromSeconds(5), options.Heartbeat);

            options.Heartbeat = TimeSpan.FromSeconds(15);
            Assert.Equal(TimeSpan.FromSeconds(15), options.Heartbeat);
        }

        [Fact]
        public void StreamName_And_ConsumerName_Can_Be_Set_And_Gotten()
        {
            var options = new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer"
            };

            Assert.Equal("stream", options.StreamName);
            Assert.Equal("consumer", options.ConsumerName);
        }

        [Fact]
        public void AutoAcknowledge_Can_Be_Set_And_Gotten()
        {
            var options = new NatsCommandQueueOptions
            {
                AutoAcknowledge = true
            };

            Assert.True(options.AutoAcknowledge);
        }

        [Fact]
        public void ValidateOptions_Throws_If_StreamName_Is_Null_Or_Whitespace()
        {
            var options = new NatsCommandQueueOptions
            {
                StreamName = null,
                ConsumerName = "consumer"
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.StreamName = "";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.StreamName = "   ";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Throws_If_ConsumerName_Is_Null_Or_Whitespace()
        {
            var options = new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = null
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.ConsumerName = "";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.ConsumerName = "   ";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Throws_If_Base_Options_Invalid()
        {
            var options = new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = null // base class property
            };

            // NatsMessageOptions.ValidateOptions throws if Subject is null or whitespace
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.Subject = "subject";
            options.NatsUrl = null; // base class property
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Does_Not_Throw_If_Valid()
        {
            var options = new NatsCommandQueueOptions
            {
                StreamName = "stream",
                ConsumerName = "consumer",
                Subject = "subject",
                NatsUrl = new Uri("nats://localhost:4222")
            };

            var ex = Record.Exception(() => options.ValidateOptions());
            Assert.Null(ex);
        }
    }
}
