using System;
using Cuemon;

namespace Savvyio.EventDriven.Messaging.CloudEvents.Cryptography
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ICloudEvent{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
    /// <seealso cref="ICloudEvent{T}" />
    public class SignedCloudEvent<T> : CloudEvent<T>, ISignedCloudEvent<T> where T : IIntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignedCloudEvent{T}"/> class.
        /// </summary>
        /// <param name="message">The cloud event to sign.</param>
        /// <param name="signature">The cryptographic signature of the <paramref name="message"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null - or -
        /// <paramref name="signature"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="signature"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public SignedCloudEvent(ICloudEvent<T> message, string signature) : base(message, message.SpecVersion)
        {
            Validator.ThrowIfNullOrWhitespace(signature);
            Signature = signature;
        }

        /// <inheritdoc />
        public string Signature { get; }
    }
}
