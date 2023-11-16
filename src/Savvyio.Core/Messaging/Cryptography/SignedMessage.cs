using System;
using Cuemon;

namespace Savvyio.Messaging.Cryptography
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ISignedMessage{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
    /// <seealso cref="ISignedMessage{T}" />
	public record SignedMessage<T> : ISignedMessage<T> where T : IRequest
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
        /// <summary>
        /// Gets the identifier of the message. When combined with <see cref="Source" />, this enables deduplication.
        /// </summary>
        /// <value>The identifier of the message.</value>
        public string Id { get; }

        /// <summary>
        /// Gets the context that describes the origin of the message. When combined with <see cref="Id" />, this enables deduplication.
        /// </summary>
        /// <value>The context that describes the origin of the message.</value>
        public string Source { get; }

        /// <summary>
        /// Gets the underlying type of the enclosed <see cref="Data" />.
        /// </summary>
        /// <value>The underlying type of the enclosed <see cref="Data" />.</value>
        public string Type { get; }

        /// <summary>
        /// Gets the time, expressed as the Coordinated Universal Time (UTC), at which this message was generated.
        /// </summary>
        /// <value>The time at which this message was generated.</value>
        public DateTime? Time { get; }

        /// <summary>
        /// Gets the payload of the message. The payload depends on the <see cref="Type" />.
        /// </summary>
        /// <value>The payload of the message.</value>
        public T Data { get; }

        /// <summary>
        /// Gets the cryptographic signature of the message.
        /// </summary>
        /// <value>The cryptographic signature of the message.</value>
        public string Signature { get; }
	}
}
