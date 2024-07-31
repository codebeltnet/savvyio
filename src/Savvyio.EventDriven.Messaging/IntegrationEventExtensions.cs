using System;
using Cuemon;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging
{
    /// <summary>
    /// Extension methods for the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    public static class IntegrationEventExtensions
    {
        /// <summary>
        /// Encloses the specified <paramref name="event"/> to an instance of <see cref="Message{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="event">The payload to attach within the message.</param>
        /// <param name="source">The context that describes the origin of the message.</param>
        /// <param name="type">The type that describes the type of <paramref name="event"/> related to the originating occurrence.</param>
        /// <param name="setup">The <see cref="MessageOptions" /> which may be configured.</param>
        /// <returns>An instance of <see cref="Message{T}"/> constraint to the <see cref="IIntegrationEvent"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="event"/> cannot be null - or -
        /// <paramref name="source"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="MessageOptions"/> in a valid state.
        /// </exception>
        public static IMessage<T> ToMessage<T>(this T @event, Uri source, string type, Action<MessageOptions> setup = null) where T : IIntegrationEvent
        {
            Validator.ThrowIfNull(@event);
            Validator.ThrowIfNull(source);
            Validator.ThrowIfNullOrWhitespace(type);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            return new Message<T>(options.MessageId, source, type, @event, options.Time);
        }
    }
}
