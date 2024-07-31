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
        /// Gets the string representation of the event identifier from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the event identifier from the <paramref name="request"/>.</returns>
        public static string GetEventId<T>(this T request) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.EventId).As<string>();
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> value of the timestamp from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The <see cref="DateTime"/> value of the timestamp from the <paramref name="request"/>.</returns>
        public static DateTime GetTimestamp<T>(this T request) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.Timestamp).As<DateTime>();
        }

        /// <summary>
        /// Gets the string representation of the type from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the type from the <paramref name="request"/>.</returns>
        public static string GetMemberType<T>(this T request) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.MemberType).As<string>();
        }
    }
}
