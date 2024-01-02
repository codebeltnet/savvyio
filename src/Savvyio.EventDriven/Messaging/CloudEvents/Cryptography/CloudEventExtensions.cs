using System;
using Cuemon;
using Cuemon.Security.Cryptography;
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
        public static ISignedCloudEvent<T> Sign<T>(this ICloudEvent<T> cloudEvent, IMarshaller marshaller, Action<SignedMessageOptions> setup = null) where T : IIntegrationEvent
        {
            Validator.ThrowIfNull(cloudEvent);
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var data = marshaller.Serialize(cloudEvent);
            var signature = KeyedHashFactory.CreateHmacCrypto(options.SignatureSecret, options.SignatureAlgorithm).ComputeHash(data).ToHexadecimalString();
            return new SignedCloudEvent<T>(cloudEvent, signature);
        }
    }
}
