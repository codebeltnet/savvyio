using System;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Events
{
    /// <summary>
    /// Extension methods for the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    public static class IntegrationEventExtensions
    {
        /// <summary>
        /// Assigns a new <paramref name="causationId"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <param name="causationId">The causation identifier of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetCausationId<T>(this T model, string causationId) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CausationId, causationId);
            return model;
        }

        /// <summary>
        /// Assigns a new <paramref name="correlationId"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <param name="correlationId">The correlation identifier of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetCorrelationId<T>(this T model, string correlationId) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CorrelationId, correlationId);
            return model;
        }

        /// <summary>
        /// Assigns a new <paramref name="eventId"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <param name="eventId">The event identifier of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetEventId<T>(this T model, string eventId) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.EventId, eventId);
            return model;
        }

        /// <summary>
        /// Assigns a new timestamp (<see cref="DateTime.UtcNow"/> value) to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetTimestamp<T>(this T model) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.Timestamp, DateTime.UtcNow);
            return model;
        }

        /// <summary>
        /// Assigns a new <paramref name="type"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <param name="type">The type of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        /// <remarks>The <paramref name="type"/> is converted to its equivalent string representation (fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly).</remarks>
        public static T SetMemberType<T>(this T model, Type type) where T : IIntegrationEvent
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.MemberType, type.ToFullNameIncludingAssemblyName());
            return model;
        }

        /// <summary>
        /// Gets the string representation of the causation identifier from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the causation identifier from the <paramref name="model"/>.</returns>
        public static string GetCausationId<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.CausationId).As<string>();
        }

        /// <summary>
        /// Gets the string representation of the correlation identifier from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="model">The <see cref="IIntegrationEvent"/> to extend.</param>
        /// <returns>The string representation of the correlation identifier from the <paramref name="model"/>.</returns>
        public static string GetCorrelationId<T>(this T model) where T : IIntegrationEvent
        {
            return MetadataFactory.Get(model, MetadataDictionary.CorrelationId).As<string>();
        }

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
