using System;
using Xunit;

namespace Savvyio.Extensions.RabbitMQ.EventDriven
{
    public class RabbitMqEventBusOptionsTest
    {
        [Fact]
        public void Constructor_Should_Set_Defaults()
        {
            var options = new RabbitMqEventBusOptions();

            Assert.Null(options.ExchangeName);
            Assert.NotNull(options.AmqpUrl); // Inherited from RabbitMqMessageOptions
            Assert.Equal(new Uri("amqp://localhost:5672"), options.AmqpUrl);
        }

        [Fact]
        public void ValidateOptions_Should_Throw_When_ExchangeName_Is_Null_Or_Empty()
        {
            var options = new RabbitMqEventBusOptions();

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.ExchangeName = "";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Should_Not_Throw_When_ExchangeName_Is_Set()
        {
            var options = new RabbitMqEventBusOptions
            {
                ExchangeName = "test-exchange"
            };

            var exception = Record.Exception(() => options.ValidateOptions());
            Assert.Null(exception);
        }
    }
}