using System;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.RabbitMQ
{
    public class RabbitMqMessageOptionsTest : Test
    {
        public RabbitMqMessageOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor_Should_Set_Default_AmqpUrl()
        {
            var options = new RabbitMqMessageOptions();

            Assert.NotNull(options.AmqpUrl);
            Assert.False(options.Persistent);
            Assert.Equal(new Uri("amqp://localhost:5672"), options.AmqpUrl);
        }

        [Fact]
        public void Persistent_Should_Be_Settable()
        {
            var options = new RabbitMqMessageOptions
            {
                Persistent = true
            };

            Assert.True(options.Persistent);
        }

        [Fact]
        public void ValidateOptions_Should_Throw_When_AmqpUrl_Is_Null()
        {
            var options = new RabbitMqMessageOptions
            {
                AmqpUrl = null
            };

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Should_Not_Throw_When_AmqpUrl_Is_Set()
        {
            var options = new RabbitMqMessageOptions
            {
                AmqpUrl = new Uri("amqp://localhost:5672")
            };

            var exception = Record.Exception(() => options.ValidateOptions());
            Assert.Null(exception);
        }
    }
}
