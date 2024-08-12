using System;
using Cuemon;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging.CloudEvents
{
    /// <summary>
    /// Extension methods for the <see cref="IMessage{T}"/> interface.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="message"/> to an <see cref="ICloudEvent{T}"/> equivalent.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="message">The payload to attach within the message.</param>
        /// <param name="specversion">The version of the CloudEvents specification which the event uses.</param>
        /// <returns>An instance of <see cref="Message{T}"/> constraint to the <see cref="IIntegrationEvent"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null.
        /// </exception>
        public static ICloudEvent<T> ToCloudEvent<T>(this IMessage<T> message, string specversion = null) where T : IIntegrationEvent
        {
            Validator.ThrowIfNull(message);
            return new CloudEvent<T>(message, specversion);
        }
    }
}
