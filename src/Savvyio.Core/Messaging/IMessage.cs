using System;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a generic way to wrap an <see cref="IRequest" /> inside a message.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IRequest"/> interface.</typeparam>
    /// <remarks>Inspired by cloudevents.io (https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md) default attributes.</remarks>
    /// <seealso cref="IAcknowledgeable"/>
    public interface IMessage<out T> : IAcknowledgeable where T : IRequest
    {
        /// <summary>
        /// Gets the identifier of the message. When combined with <see cref="Source"/>, this enables deduplication.
        /// </summary>
        /// <value>The identifier of the message.</value>
        string Id { get; }

        /// <summary>
        /// Gets the context that describes the origin of the message. When combined with <see cref="Id"/>, this enables deduplication.
        /// </summary>
        /// <value>The context that describes the origin of the message.</value>
        public string Source { get; }

        /// <summary>
        /// Gets the type that describes the type of event related to the originating occurrence.
        /// </summary>
        /// <value>The type that describes the type of event related to the originating occurrence.</value>
        string Type { get; }

        /// <summary>
        /// Gets the time, expressed as the Coordinated Universal Time (UTC), at which this message was generated.
        /// </summary>
        /// <value>The time at which this message was generated.</value>
        DateTime? Time { get; }

        /// <summary>
        /// Gets the payload of the message.
        /// </summary>
        /// <value>The payload of the message.</value>
        T Data { get; }
    }
}
