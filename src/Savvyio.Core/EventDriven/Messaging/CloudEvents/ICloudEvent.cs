using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging.CloudEvents
{
    /// <summary>
    /// Defines a generic way to wrap an <see cref="IRequest" /> inside a CloudEvents compliant message format.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
    /// <seealso cref="IMessage{T}" />
    /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md</remarks>
    public interface ICloudEvent<out T> : IMessage<T> where T : IIntegrationEvent
    {
        /// <summary>
        /// Gets version of the CloudEvents specification which the event uses.
        /// </summary>
        /// <value>The version of the CloudEvents specification which the event uses.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#specversion</remarks>
        string Specversion { get; }
    }
}
