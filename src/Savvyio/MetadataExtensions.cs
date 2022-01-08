using System;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="IMetadata"/> interface.
    /// </summary>
    public static class MetadataExtensions
    {
        /// <summary>
        /// Gets the string representation of the causation identifier from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>The string representation of the causation identifier from the <paramref name="model"/>.</returns>
        public static string GetCausationId<T>(this T model) where T : IMetadata
        {
            return MetadataFactory.Get(model, MetadataDictionary.CausationId).As<string>();
        }

        /// <summary>
        /// Gets the string representation of the correlation identifier from the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>The string representation of the correlation identifier from the <paramref name="model"/>.</returns>
        public static string GetCorrelationId<T>(this T model) where T : IMetadata
        {
            return MetadataFactory.Get(model, MetadataDictionary.CorrelationId).As<string>();
        }

        /// <summary>
        /// Assigns a new <paramref name="causationId"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="causationId">The causation identifier of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetCausationId<T>(this T model, string causationId) where T : IMetadata
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CausationId, causationId);
            return model;
        }

        /// <summary>
        /// Assigns a new <paramref name="correlationId"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="correlationId">The correlation identifier of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetCorrelationId<T>(this T model, string correlationId) where T : IMetadata
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.CorrelationId, correlationId);
            return model;
        }

        /// <summary>
        /// Assigns a new <paramref name="eventId"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="eventId">The event identifier of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetEventId<T>(this T model, string eventId) where T : IMetadata
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.EventId, eventId);
            return model;
        }

        /// <summary>
        /// Assigns a new timestamp (<see cref="DateTime.UtcNow"/> value) to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static T SetTimestamp<T>(this T model) where T : IMetadata
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.Timestamp, DateTime.UtcNow);
            return model;
        }

        /// <summary>
        /// Assigns a new <paramref name="type"/> to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="type">The type of the model.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        /// <remarks>The <paramref name="type"/> is converted to its equivalent string representation (fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly).</remarks>
        public static T SetMemberType<T>(this T model, Type type) where T : IMetadata
        {
            MetadataFactory.SetUnrestricted(model, MetadataDictionary.MemberType, type.ToFullNameIncludingAssemblyName());
            return model;
        }

        /// <summary>
        /// Add or update a set of model to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to add or update.</param>
        /// <param name="value">The value of the element to add or update.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        /// <exception cref="ReservedKeywordException">
        /// <paramref name="key"/> is a reserved keyword.
        /// </exception>
        public static T SaveMetadata<T>(this T model, string key, object value) where T : IMetadata
        {
            MetadataFactory.Set(model, key, value);
            return model;
        }

        /// <summary>
        /// Copies model from the <paramref name="source"/> to the <paramref name="destination"/> if not already existing.
        /// </summary>
        /// <typeparam name="TSource">The giving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <typeparam name="TDestination">The receiving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="destination">The <see cref="IMetadata"/> to extend. Receives model from <paramref name="source"/>.</param>
        /// <param name="source">The model that will give metata to <paramref name="destination"/>.</param>
        /// <returns>A reference to <paramref name="destination"/> after the operation has completed.</returns>
        public static TDestination MergeMetadata<TSource, TDestination>(this TDestination destination, TSource source)
            where TSource : IMetadata
            where TDestination : IMetadata
        {
            if (destination != null && source != null)
            {
                foreach (var entry in source.Metadata)
                {
                    if (!destination.Metadata.ContainsKey(entry.Key))
                    {
                        destination.Metadata.AddUnristricted(entry.Key, entry.Value); // bypass reserved keyword check since the value is only added if non-existing
                    }
                }
            }
            return destination;
        }
    }
}
