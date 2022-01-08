using System;
using Cuemon.Extensions;

namespace Savvyio.EventDriven
{
    /// <summary>
    /// Extension methods for the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    public static class IntegrationEventExtensions
    {
        /// <summary>
        /// Gets the string representation of the event identifier from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the event identifier from the <paramref name="model"/>.</returns>
        public static string GetEventId<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.EventId).As<string>();
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> value of the timestamp from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The <see cref="DateTime"/> value of the timestamp from the <paramref name="model"/>.</returns>
        public static DateTime GetTimestamp<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.Timestamp).As<DateTime>();
        }

        /// <summary>
        /// Gets the string representation of the type from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the type from the <paramref name="model"/>.</returns>
        public static string GetMemberType<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.MemberType).As<string>();
        }
    }
}
