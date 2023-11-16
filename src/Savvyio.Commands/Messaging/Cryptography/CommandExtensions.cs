using System;
using Cuemon;
using Cuemon.Security.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.Commands.Messaging.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="ICommand"/> interface.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="command"/> to an <see cref="ISignedMessage{T}"/> equivalent using the provided <paramref name="setup"/> configurator.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="ICommand"/> interface.</typeparam>
        /// <param name="command">The <see cref="ICommand"/> message to cryptographically sign.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions{T}" /> that must be configured.</param>
        /// <returns>An implementation of the <see cref="ISignedMessage{T}"/> interface having a constraint to the <see cref="ICommand"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="command"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions{T}"/> in a valid state.
        /// </exception>
        public static ISignedMessage<T> Sign<T>(this IMessage<T> command, Action<SignedMessageOptions<T>> setup) where T : ICommand
        {
            Validator.ThrowIfNull(command);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var message = options.SerializerFactory(command);
            var signature = KeyedHashFactory.CreateHmacCrypto(options.SignatureSecret, options.SignatureAlgorithm).ComputeHash(message).ToHexadecimalString();
            return new SignedMessage<T>(command, signature);
        }
    }
}
