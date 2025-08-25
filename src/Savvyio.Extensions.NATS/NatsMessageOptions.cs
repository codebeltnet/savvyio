using System;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.NATS
{
    /// <summary>
    /// Configuration options that is related to NATS.
    /// </summary>
    /// <seealso cref="IValidatableParameterObject" />
    public class NatsMessageOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NatsMessageOptions"/> class with default values.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="NatsMessageOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="NatsUrl"/></term>
        ///         <description><c>new Uri("nats://127.0.0.1:4222")</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Subject"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public NatsMessageOptions()
        {
            NatsUrl = new Uri("nats://127.0.0.1:4222");
        }

        /// <summary>
        /// Gets or sets the URI of the NATS server.
        /// </summary>
        public Uri NatsUrl { get; set; }

        /// <summary>
        /// Gets or sets the subject to publish or subscribe to in NATS.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Validates the current options and throws an exception if the state is invalid.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Subject"/> is null or whitespace - or -
        /// <see cref="NatsUrl"/> is null.
        /// </exception>
        public virtual void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(NatsUrl == null);
            Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(Subject));
        }
    }
}
