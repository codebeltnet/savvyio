using System;
using Cuemon;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IMessage{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
    /// <seealso cref="IMessage{T}" />
    public record Message<T> : IMessage<T> where T : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the message.</param>
        /// <param name="source">The context that describes the origin of the message.</param>
        /// <param name="data">The payload of the message.</param>
        /// <param name="type">The underlying type of the enclosed <paramref name="data"/>.</param>
        /// <param name="time">The time at which this message was generated.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="id"/> cannot be null - or -
        /// <paramref name="source"/> cannot be null - or -
        /// <paramref name="data"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="id"/> cannot be empty or consist only of white-space characters - or -
        /// <paramref name="time"/> was not expressed as the Coordinated Universal Time (UTC).
        /// </exception>
        public Message(string id, Uri source, T data, Type type = null, DateTime? time = null)
        {
            Validator.ThrowIfNullOrWhitespace(id);
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNull(data);
            Validator.ThrowIfTrue(() => time.HasValue && time.Value.Kind != DateTimeKind.Utc, nameof(time), "Time must be expressed as the Coordinated Universal Time (UTC).");
            Id = id;
            Source = source.OriginalString;
            Data = data;
            Type = (type ?? data.GetType()).ToFullNameIncludingAssemblyName();
            Time = (time ?? DateTime.UtcNow);
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
        /// Gets the time, expressed as the Coordinated Universal Time (UTC), at which the message was generated.
        /// </summary>
        /// <value>The time at which the message was generated.</value>
        public DateTime? Time { get; }

        /// <summary>
        /// Gets the payload of the message. The payload depends on the <see cref="Type" />.
        /// </summary>
        /// <value>The payload of the message.</value>
        public T Data { get; }
    }
}
