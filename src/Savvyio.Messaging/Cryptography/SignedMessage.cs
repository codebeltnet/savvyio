using System;
using Cuemon;

namespace Savvyio.Messaging.Cryptography
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ISignedMessage{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
    /// <seealso cref="ISignedMessage{T}" />
	public record SignedMessage<T> : Acknowledgeable, ISignedMessage<T> where T : IRequest
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="SignedMessage{T}"/> class.
        /// </summary>
        /// <param name="message">The message to cryptographically sign.</param>
        /// <param name="signature">The cryptographic signature of the <paramref name="message"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null - or -
        /// <paramref name="signature"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="signature"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public SignedMessage(IMessage<T> message, string signature)
		{
			Validator.ThrowIfNull(message);
			Validator.ThrowIfNullOrWhitespace(signature);
			Id = message.Id;
			Source = message.Source;
			Type = message.Type;
			Time = message.Time;
			Data = message.Data;
			Signature = signature;
		}

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Source { get; }

        /// <inheritdoc />
        public string Type { get; }

        /// <inheritdoc />
        public DateTime? Time { get; }

        /// <inheritdoc />
        public T Data { get; }

        /// <summary>
        /// Gets the cryptographic signature of the message.
        /// </summary>
        /// <value>The cryptographic signature of the message.</value>
        public string Signature { get; }
	}
}
