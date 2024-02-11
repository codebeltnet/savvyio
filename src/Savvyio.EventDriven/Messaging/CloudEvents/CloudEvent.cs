using System;
using Cuemon;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging.CloudEvents
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ICloudEvent{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
    /// <seealso cref="ICloudEvent{T}" />
    public class CloudEvent<T> : ICloudEvent<T> where T : IIntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudEvent{T}"/> class.
        /// </summary>
        /// <param name="message">The message to elevate to an <see cref="ICloudEvent{T}"/> compliance.</param>
        /// <param name="specVersion">The version of the CloudEvents specification which the event uses.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null.
        /// </exception>
        public CloudEvent(IMessage<T> message, string specVersion = null)
        {
            Validator.ThrowIfNull(message);
            Id = message.Id;
            Source = message.Source;
            Type = message.Type;
            Time = message.Time;
            Data = message.Data;
            SpecVersion = specVersion ?? "1.0";
        }


        /// <summary>
        /// Gets the identifier of the event. When combined with <see cref="Source"/>, this enables deduplication.
        /// </summary>
        /// <value>The identifier of the event.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#id</remarks>
        public string Id { get; }

        /// <summary>
        /// Gets the context in which an event happened. When combined with <see cref="Id"/>, this enables deduplication.
        /// </summary>
        /// <value>The context in which an event happened.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#source-1</remarks>
        public string Source { get; }


        /// <summary>
        /// Gets the value describing the type of event related to the originating occurrence.
        /// </summary>
        /// <value>The value describing the type of event related to the originating occurrence.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#type</remarks>
        public string Type { get; }
        
        /// <summary>
        /// Gets the time, expressed as the Coordinated Universal Time (UTC), of when the occurrence happened.
        /// </summary>
        /// <value>The timestamp of when the occurrence happened.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#time</remarks>
        public DateTime? Time { get; }


        /// <summary>
        /// Gets the event payload.
        /// </summary>
        /// <value>The event payload.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#event-data</remarks>
        public T Data { get; }

        /// <inheritdoc />
        public string SpecVersion { get; }
    }
}
