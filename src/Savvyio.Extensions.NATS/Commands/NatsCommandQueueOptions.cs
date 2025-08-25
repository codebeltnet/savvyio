using System;
using Cuemon;

namespace Savvyio.Extensions.NATS.Commands
{
    /// <summary>
    /// Configuration options for <see cref="NatsCommandQueue"/>.
    /// </summary>
    /// <seealso cref="NatsMessageOptions"/>
    public class NatsCommandQueueOptions : NatsMessageOptions
    {
        private int _maxMessages;
        private TimeSpan _expires;

        /// <summary>
        /// Initializes a new instance of the <see cref="NatsCommandQueueOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="NatsCommandQueueOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="MaxMessages"/></term>
        ///         <description>100</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="AutoAcknowledge"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Expires"/></term>
        ///         <description><c>TimeSpan.Zero</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Heartbeat"/></term>
        ///         <description><c>TimeSpan.Zero</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ConsumerName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StreamName"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public NatsCommandQueueOptions()
        {
            MaxMessages = 25;
            Heartbeat = TimeSpan.Zero;
            Expires = TimeSpan.Zero;
        }

        /// <summary>
        /// Gets or sets the maximum number of messages to process.
        /// </summary>
        public int MaxMessages
        {
            get => _maxMessages;
            set => _maxMessages = Math.Clamp(value, 1, short.MaxValue);
        }

        /// <summary>
        /// Gets or sets the expiration time for messages in the queue.
        /// </summary>
        /// <remarks>If set to a value greater than or equal to 30 seconds, the <see cref="Heartbeat"/> property is automatically set to 5 seconds.</remarks>
        public TimeSpan Expires
        {
            get => _expires;
            set
            {
                _expires = value;
                if (_expires >= TimeSpan.FromSeconds(30))
                {
                    Heartbeat = TimeSpan.FromSeconds(5);
                }
            }
        }

        /// <summary>
        /// Gets or sets the interval for the heartbeat signal sent from the NATS server.
        /// </summary>
        /// <remarks>If <see cref="Expires"/> is set to a value greater than or equal to 30 seconds, this property is automatically set to 5 seconds.</remarks>
        public TimeSpan Heartbeat { get; set; }

        /// <summary>
        /// Gets or sets the name of the NATS stream.
        /// </summary>
        public string StreamName { get; set; }

        /// <summary>
        /// Gets or sets the name of the NATS consumer.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether messages should be automatically acknowledged.
        /// </summary>
        public bool AutoAcknowledge { get; set; }

        /// <summary>
        /// Validates the options for the NATS command queue.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="StreamName"/> or <see cref="ConsumerName"/> is null or whitespace.
        /// </exception>
        public override void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(StreamName));
            Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(ConsumerName));
            base.ValidateOptions();
        }
    }
}
