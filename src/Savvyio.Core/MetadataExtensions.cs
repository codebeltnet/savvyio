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
        /// Gets the string representation of the causation identifier from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>The string representation of the causation identifier from the <paramref name="request"/>.</returns>
        public static string GetCausationId<T>(this T request) where T : IMetadata
        {
            return MetadataFactory.Get(request, MetadataDictionary.CausationId).As<string>();
        }

        /// <summary>
        /// Gets the string representation of the correlation identifier from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>The string representation of the correlation identifier from the <paramref name="request"/>.</returns>
        public static string GetCorrelationId<T>(this T request) where T : IMetadata
        {
            return MetadataFactory.Get(request, MetadataDictionary.CorrelationId).As<string>();
        }

        /// <summary>
        /// Gets the string representation of the request identifier from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>The string representation of the request identifier from the <paramref name="request"/>.</returns>
        public static string GetRequestId<T>(this T request) where T : IMetadata
        {
            return MetadataFactory.Get(request, MetadataDictionary.RequestId).As<string>();
        }

        /// <summary>
        /// Gets the string representation of the underlying member type of <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <returns>The string representation of the member type from the <paramref name="request"/>.</returns>
        /// <remarks>The underlying <see cref="Type"/> of a model expressed as a string representation (fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly).</remarks>
        public static string GetMemberType<T>(this T request) where T : IMetadata
        {
            return MetadataFactory.Get(request, MetadataDictionary.MemberType).As<string>();
        }

        /// <summary>
        /// Assigns a new <paramref name="causationId"/> to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="causationId">The causation identifier of the model.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        public static T SetCausationId<T>(this T request, string causationId) where T : IMetadata
        {
            MetadataFactory.Set(request, MetadataDictionary.CausationId, causationId);
            return request;
        }

        /// <summary>
        /// Assigns a new <paramref name="correlationId"/> to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="correlationId">The correlation identifier of the model.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        public static T SetCorrelationId<T>(this T request, string correlationId) where T : IMetadata
        {
            MetadataFactory.Set(request, MetadataDictionary.CorrelationId, correlationId);
            return request;
        }

        /// <summary>
        /// Assigns a new <paramref name="requestId"/> to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="requestId">The request identifier of the model.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        public static T SetRequestId<T>(this T request, string requestId) where T : IMetadata
        {
            MetadataFactory.Set(request, MetadataDictionary.RequestId, requestId);
            return request;
        }

        /// <summary>
        /// Assigns a new <paramref name="eventId"/> to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="eventId">The event identifier of the model.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        public static T SetEventId<T>(this T request, string eventId) where T : IMetadata
        {
            MetadataFactory.Set(request, MetadataDictionary.EventId, eventId);
            return request;
        }

        /// <summary>
        /// Assigns a new timestamp (<see cref="DateTime.UtcNow"/> value) to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="utcTimestamp">The optional <see cref="DateTime"/> value expressed as the Coordinated Universal Time (UTC).</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="utcTimestamp"/> (when set) was not expressed as the Coordinated Universal Time (UTC).
        /// </exception>
        public static T SetTimestamp<T>(this T request, DateTime? utcTimestamp = null) where T : IMetadata
        {
            Validator.ThrowIfTrue(utcTimestamp.HasValue && utcTimestamp.Value.Kind != DateTimeKind.Utc, nameof(utcTimestamp), "Value must be expressed as the Coordinated Universal Time (UTC).");
            MetadataFactory.Set(request, MetadataDictionary.Timestamp, utcTimestamp ?? DateTime.UtcNow);
            return request;
        }

        /// <summary>
        /// Assigns a new <paramref name="type"/> to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="type">The type of the model.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        /// <remarks>The <paramref name="type"/> is converted to its equivalent string representation (fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly).</remarks>
        public static T SetMemberType<T>(this T request, Type type) where T : IMetadata
        {
            MetadataFactory.Set(request, MetadataDictionary.MemberType, type.ToFullNameIncludingAssemblyName());
            return request;
        }

        /// <summary>
        /// Add or update a set of model to the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to add or update.</param>
        /// <param name="value">The value of the element to add or update.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request" /> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        public static T SaveMetadata<T>(this T request, string key, object value) where T : IMetadata
        {
            Validator.ThrowIfNull(request);
            Validator.ThrowIfNull(key);

            var parsedKey = MetadataDictionary.EnsureReservedKeywordCapitalization(key);
            MetadataFactory.Set(request, parsedKey, value);
            return request;
        }

        /// <summary>
        /// Copies model from the <paramref name="source"/> to the <paramref name="destination"/> if not already existing.
        /// </summary>
        /// <typeparam name="TSource">The giving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <typeparam name="TDestination">The receiving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="destination">The <see cref="IMetadata"/> to extend. Receives model from <paramref name="source"/>.</param>
        /// <param name="source">The model that will give metadata to <paramref name="destination"/>.</param>
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
                        destination.Metadata.Add(entry.Key, entry.Value);
                    }
                }
            }
            return destination;
        }
    }
}
