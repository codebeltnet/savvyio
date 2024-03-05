using System;
using Cuemon;
using Cuemon.Security.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.EventDriven.Messaging.CloudEvents.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="ICloudEvent{T}"/> interface.
    /// </summary>
    public static class CloudEventExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="cloudEvent"/> to an <see cref="ISignedCloudEvent{T}"/> equivalent.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="cloudEvent">The payload to attach within the message.</param>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting the <paramref name="cloudEvent"/> into an <see cref="ISignedCloudEvent{T}"/>.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions" /> which may be configured.</param>
        /// <returns>An instance of <see cref="SignedCloudEvent{T}"/> constraint to the <see cref="IIntegrationEvent"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cloudEvent"/> cannot be null -or-
        /// <paramref name="marshaller"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions"/> in a valid state.
        /// </exception>
        public static ISignedCloudEvent<T> SignCloudEvent<T>(this ICloudEvent<T> cloudEvent, IMarshaller marshaller, Action<SignedMessageOptions> setup = null) where T : IIntegrationEvent
        {
            Validator.ThrowIfNull(cloudEvent);
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var data = marshaller.Serialize(cloudEvent);
            var signature = KeyedHashFactory.CreateHmacCrypto(options.SignatureSecret, options.SignatureAlgorithm).ComputeHash(data).ToHexadecimalString();
            return new SignedCloudEvent<T>(cloudEvent, signature);
        }

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
            var baseCloudEvent = SignCloudEvent(cloudEvent.Clone(() => new CloudEvent<T>(cloudEvent, cloudEvent.SpecVersion)) as ICloudEvent<T>, marshaller, setup);
            if (!cloudEvent.Signature.Equals(baseCloudEvent.Signature))
            {
                throw new ArgumentOutOfRangeException(nameof(cloudEvent), cloudEvent.Signature, "The signature of the cloud event does not match the cryptographically calculated value. Either you are using an incorrect secret and/or algorithm or the message has been tampered with.");
            }
        }
    }
}
