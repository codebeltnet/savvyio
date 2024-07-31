using System;
using Cuemon;

namespace Savvyio.Messaging.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="ISignedMessage{T}"/> interface.
    /// </summary>
    public static class SignedMessageExtensions
    {
        /// <summary>
        /// Verifies the digital signature of the <see cref="ISignedMessage{T}"/> message.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
        /// <param name="message">The <see cref="IRequest"/> message to cryptographically verify.</param>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting the <paramref name="message"/> into an <see cref="ISignedMessage{T}"/>.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions" /> that must be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null -or-
        /// <paramref name="marshaller"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions"/> in a valid state.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="message"/> signature did not match the cryptographically calculated value.
        /// </exception>
        /// <remarks>This method throws an <see cref="ArgumentOutOfRangeException"/> if the verification of the digital signature fails.</remarks>
        public static void CheckSignature<T>(this ISignedMessage<T> message, IMarshaller marshaller, Action<SignedMessageOptions> setup) where T : IRequest
        {
            Validator.ThrowIfNull(message);
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidConfigurator(setup, out _);
            var baseMessage = message.Clone().Sign(marshaller, setup);
            if (!message.Signature.Equals(baseMessage.Signature))
            {
                throw new ArgumentOutOfRangeException(nameof(message), message.Signature, "The signature of the message does not match the cryptographically calculated value. Either you are using an incorrect secret and/or algorithm or the message has been tampered with.");
            }
        }
    }
}
