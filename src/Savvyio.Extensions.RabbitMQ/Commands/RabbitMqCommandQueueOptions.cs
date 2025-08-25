using System;
using Cuemon;

namespace Savvyio.Extensions.RabbitMQ.Commands
{
    /// <summary>
    /// Configuration options for <see cref="RabbitMqCommandQueue"/>.
    /// </summary>
    /// <seealso cref="RabbitMqMessageOptions"/>
    public class RabbitMqCommandQueueOptions : RabbitMqMessageOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMqCommandQueueOptions"/> class with default values.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="RabbitMqCommandQueueOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="QueueName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AutoAcknowledge"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Durable"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Exclusive"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AutoDelete"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public RabbitMqCommandQueueOptions()
        {
        }

        /// <summary>
        /// Gets or sets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the RabbitMQ queue to be used for command messages.
        /// </value>
        public string QueueName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether messages should be automatically acknowledged.
        /// </summary>
        /// <value>
        /// <c>true</c> if messages are automatically acknowledged; otherwise, <c>false</c>.
        /// </value>
        public bool AutoAcknowledge { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the queue should be durable.
        /// </summary>
        /// <value><c>true</c> if the queue should survive broker restarts; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Durable queues remain declared after a RabbitMQ broker restart, but messages in non-persistent queues will be lost.
        /// </remarks>
        public bool Durable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the queue should be exclusive.
        /// </summary>
        /// <value><c>true</c> if the queue is used by only one connection and deleted when that connection closes; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Exclusive queues are only accessible by the connection that declares them and are deleted when that connection closes.
        /// </remarks>
        public bool Exclusive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the queue should be automatically deleted when no longer in use.
        /// </summary>
        /// <value><c>true</c> if the queue is automatically deleted when the last consumer unsubscribes; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// Auto-delete queues are deleted when the last consumer unsubscribes. If there are no consumers, the queue is not deleted.
        /// </remarks>
        public bool AutoDelete { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="QueueName"/> cannot be null or empty.
        /// </exception>
        public override void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(QueueName));
            base.ValidateOptions();
        }
    }
}
