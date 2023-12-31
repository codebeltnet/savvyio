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
        /// <param name="serializerContext">The <see cref="ISerializerContext"/> that is used when converting the <paramref name="command"/> into an <see cref="ISignedMessage{T}"/>.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions" /> that must be configured.</param>
        /// <returns>An implementation of the <see cref="ISignedMessage{T}"/> interface having a constraint to the <see cref="ICommand"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="command"/> cannot be null -or-
        /// <paramref name="serializerContext"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions"/> in a valid state.
        /// </exception>
        public static ISignedMessage<T> Sign<T>(this IMessage<T> command, ISerializerContext serializerContext, Action<SignedMessageOptions> setup) where T : ICommand
        {
            Validator.ThrowIfNull(command);
            Validator.ThrowIfNull(serializerContext);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var message = serializerContext.Serialize(command);
            var signature = KeyedHashFactory.CreateHmacCrypto(options.SignatureSecret, options.SignatureAlgorithm).ComputeHash(message).ToHexadecimalString();
            return new SignedMessage<T>(command, signature);
        }
    }
}
