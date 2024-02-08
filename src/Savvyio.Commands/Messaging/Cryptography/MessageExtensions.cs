using System;
using Cuemon;
using Cuemon.Security.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.Commands.Messaging.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="IMessage{T}"/> interface.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="message"/> to an <see cref="ISignedMessage{T}"/> equivalent using the provided <paramref name="setup"/> configurator.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="ICommand"/> interface.</typeparam>
        /// <param name="message">The <see cref="ICommand"/> message to cryptographically sign.</param>
        /// <param name="marshaller">The <see cref="IMarshaller"/> that is used when converting the <paramref name="message"/> into an <see cref="ISignedMessage{T}"/>.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions" /> that must be configured.</param>
        /// <returns>An implementation of the <see cref="ISignedMessage{T}"/> interface having a constraint to the <see cref="ICommand"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null -or-
        /// <paramref name="marshaller"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions"/> in a valid state.
        /// </exception>
        public static ISignedMessage<T> Sign<T>(this IMessage<T> message, IMarshaller marshaller, Action<SignedMessageOptions> setup) where T : ICommand
        {
            Validator.ThrowIfNull(message);
            Validator.ThrowIfNull(marshaller);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var data = marshaller.Serialize(message);
            var signature = KeyedHashFactory.CreateHmacCrypto(options.SignatureSecret, options.SignatureAlgorithm).ComputeHash(data).ToHexadecimalString();
            return new SignedMessage<T>(message, signature);
        }
    }
}
