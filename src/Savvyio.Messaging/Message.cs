using System;
using Cuemon;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IMessage{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
    /// <seealso cref="IMessage{T}" />
    public record Message<T> : Acknowledgeable, IMessage<T> where T : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the message.</param>
        /// <param name="source">The context that describes the origin of the message.</param>
        /// <param name="data">The payload of the message.</param>
        /// <param name="type">The type that describes the type of event related to the originating occurrence.</param>
        /// <param name="time">The time at which this message was generated.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="id"/> cannot be null - or -
        /// <paramref name="source"/> cannot be null - or -
        /// <paramref name="type"/> cannot be null - or -
        /// <paramref name="data"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="id"/> cannot be empty or consist only of white-space characters - or -
        /// <paramref name="type"/> cannot be empty or consist only of white-space characters - or -
        /// <paramref name="time"/> was not expressed as the Coordinated Universal Time (UTC).
        /// </exception>
        public Message(string id, Uri source, string type, T data, DateTime? time = null)
        {
            Validator.ThrowIfNullOrWhitespace(id);
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNullOrWhitespace(type);
            Validator.ThrowIfNull(data);
            Validator.ThrowIfTrue(() => time.HasValue && time.Value.Kind != DateTimeKind.Utc, nameof(time), "Time must be expressed as the Coordinated Universal Time (UTC).");
            Id = id;
            Source = source.OriginalString;
            Type = type;
            Data = data;
            Time = (time ?? DateTime.UtcNow);
        }

        internal Message()
        {
        }

        /// <inheritdoc />
        public string Id { get; internal set; }

        /// <inheritdoc />
        public string Source { get; internal set; }

        /// <inheritdoc />
        public string Type { get; internal set; }

        /// <inheritdoc />
        public DateTime? Time { get; internal set; }

        /// <inheritdoc />
        public T Data { get; internal set; }
    }
}
