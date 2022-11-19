namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a generic way to wrap an <see cref="IRequest"/> inside a message.
    /// </summary>
    /// <remarks>Inspired by cloudevents.io (https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md) default attributes.</remarks>
    public interface IMessage<out TData> where TData : IRequest
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
        /// Gets the underlying type of the enclosed <see cref="Data"/>.
        /// </summary>
        /// <value>The underlying type of the enclosed <see cref="Data"/>.</value>
        string Type { get; }

        /// <summary>
        /// Gets the time, expressed as the Coordinated Universal Time (UTC), at which this message was generated.
        /// </summary>
        /// <value>The time at which this message was generated.</value>
        string Time { get; }

        /// <summary>
        /// Gets the payload of the message. The payload depends on the <see cref="Type"/>.
        /// </summary>
        /// <value>The payload of the message.</value>
        TData Data { get; }
    }
}
