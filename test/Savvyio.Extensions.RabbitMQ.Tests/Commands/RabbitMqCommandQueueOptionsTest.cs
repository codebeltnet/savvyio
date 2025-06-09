using System;
using Xunit;

namespace Savvyio.Extensions.RabbitMQ.Commands
{
    public class RabbitMqCommandQueueOptionsTest
    {
        [Fact]
        public void Constructor_Should_Set_Defaults()
        {
            var options = new RabbitMqCommandQueueOptions();

            Assert.Null(options.QueueName);
            Assert.False(options.AutoAcknowledge);
            Assert.False(options.Durable);
            Assert.False(options.Exclusive);
            Assert.False(options.AutoDelete);
            Assert.NotNull(options.AmqpUrl); // Inherited from RabbitMqMessageOptions
        }

        [Fact]
        public void ValidateOptions_Should_Throw_When_QueueName_Is_Null_Or_Empty()
        {
            var options = new RabbitMqCommandQueueOptions();

            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());

            options.QueueName = "";
            Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
        }

        [Fact]
        public void ValidateOptions_Should_Not_Throw_When_QueueName_Is_Set()
        {
            var options = new RabbitMqCommandQueueOptions
            {
                QueueName = "test-queue"
            };

            var exception = Record.Exception(() => options.ValidateOptions());
            Assert.Null(exception);
        }
    }
}
