using System;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.RabbitMQ
{
    /// <summary>
    /// Configuration options that is related to RabbitMQ.
    /// </summary>
    /// <seealso cref="IValidatableParameterObject" />
    public class RabbitMqMessageOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqMessageOptions"/> class with default values.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AmazonMessageOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="AmqpUrl"/></term>
        ///         <description><c>new Uri("amqp://localhost:5672")</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public RabbitMqMessageOptions()
        {
            AmqpUrl = new Uri("amqp://localhost:5672");
        }

        /// <summary>
        /// Gets or sets the AMQP URL used to connect to the RabbitMQ broker.
        /// </summary>
        /// <value>
        /// The <see cref="Uri"/> representing the AMQP endpoint for the RabbitMQ broker.
        /// </value>
        public Uri AmqpUrl { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="AmqpUrl"/> cannot be null.
        /// </exception>
        public virtual void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(AmqpUrl == null);
        }
    }
}
