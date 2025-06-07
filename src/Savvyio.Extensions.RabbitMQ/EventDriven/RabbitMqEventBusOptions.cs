using System;
using Cuemon;

namespace Savvyio.Extensions.RabbitMQ.EventDriven
{
    /// <summary>
    /// Configuration options for <see cref="RabbitMqEventBus"/>.
    /// </summary>
    /// <seealso cref="RabbitMqMessageOptions"/>
    public class RabbitMqEventBusOptions : RabbitMqMessageOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqEventBusOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="RabbitMqEventBusOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ExchangeName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public RabbitMqEventBusOptions()
        {
        }

        /// <summary>
        /// Gets or sets the name of the exchange.
        /// </summary>
        /// <value>The name of the exchange.</value>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="ExchangeName"/> cannot be null or empty.
        /// </exception>
        public override void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(string.IsNullOrEmpty(ExchangeName));
            base.ValidateOptions();
        }
    }
}
