using System;
using Cuemon;
using Cuemon.Configuration;
using Cuemon.Extensions;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Configuration options that is related to wrapping an <see cref="IRequest"/> implementation inside a message.
    /// </summary>
    /// <seealso cref="IValidatableParameterObject" />
    public class MessageOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="MessageOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="MessageId"/></term>
        ///         <description><c>Guid.NewGuid().ToString("N")</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public MessageOptions()
        {
            MessageId = Guid.NewGuid().ToString("N");
            Time = null;
        }

        /// <summary>
        /// Gets or sets the identifier of the message.
        /// </summary>
        /// <value>The identifier of the message.</value>
        public string MessageId { get; set; }

        /// <summary>
        /// Gets or sets the time, expressed as the Coordinated Universal Time (UTC), of the message.
        /// </summary>
        /// <value>The point in time the message was generated.</value>
        public DateTime? Time { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        /// <exception cref="InvalidOperationException">
        /// <see cref="MessageId"/> cannot be null, empty or consist only of white-space characters  - or -
        /// <see cref="Time"/> (when set) was not expressed as the Coordinated Universal Time (UTC).
        /// </exception>
        public virtual void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(MessageId.IsNullOrWhiteSpace());
            Validator.ThrowIfInvalidState(Time.HasValue && Time.Value.Kind != DateTimeKind.Utc);
        }
    }
}
