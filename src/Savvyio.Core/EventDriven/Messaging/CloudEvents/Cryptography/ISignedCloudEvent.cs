using Savvyio.Messaging.Cryptography;

namespace Savvyio.EventDriven.Messaging.CloudEvents.Cryptography
{
    /// <summary>
    /// Defines a generic way to wrap an <see cref="IRequest" /> inside a CloudEvents compliant message format.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
    /// <seealso cref="ISignedMessage{T}" />
    /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md</remarks>
    public interface ISignedCloudEvent<out T> : ICloudEvent<T>, ISignedMessage<T> where T : IIntegrationEvent
    {
    }
}
