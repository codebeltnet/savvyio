using System;
using Cuemon;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.EventDriven.Messaging.CloudEvents.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="ICloudEvent{T}"/> interface.
    /// </summary>
    public static class SignedCloudEventExtensions
    {

        /// <summary>
        /// Verifies the digital signature of the <see cref="ISignedCloudEvent{T}"/> message.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
        /// <param name="cloudEvent">The <see cref="IRequest"/> message to cryptographically verify.</param>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting the <paramref name="cloudEvent"/> into an <see cref="ISignedCloudEvent{T}"/>.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions" /> that must be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cloudEvent"/> cannot be null -or-
        /// <paramref name="marshaller"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions"/> in a valid state.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="cloudEvent"/> signature did not match the cryptographically calculated value.
        /// </exception>
        /// <remarks>This method throws an <see cref="ArgumentOutOfRangeException"/> if the verification of the digital signature fails.</remarks>
        public static void CheckCloudEventSignature<T>(this ISignedCloudEvent<T> cloudEvent, IMarshaller marshaller, Action<SignedMessageOptions> setup) where T : IIntegrationEvent
        {
            Validator.ThrowIfNull(cloudEvent);
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidConfigurator(setup, out _);
            var baseCloudEvent = (cloudEvent.Clone(() => new CloudEvent<T>(cloudEvent, cloudEvent.Specversion)) as ICloudEvent<T>).SignCloudEvent(marshaller, setup);
            if (!cloudEvent.Signature.Equals(baseCloudEvent.Signature))
            {
                throw new ArgumentOutOfRangeException(nameof(cloudEvent), cloudEvent.Signature, "The signature of the cloud event does not match the cryptographically calculated value. Either you are using an incorrect secret and/or algorithm or the message has been tampered with.");
            }
        }
    }
}
