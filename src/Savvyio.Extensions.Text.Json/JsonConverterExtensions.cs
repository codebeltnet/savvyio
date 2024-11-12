using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Cuemon;
using Savvyio.Domain;
using Savvyio.Extensions.Text.Json.Converters;
using Savvyio.Messaging;

namespace Savvyio.Extensions.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonConverter"/> class.
    /// </summary>
    public static class JsonConverterExtensions
    {
        /// <summary>
        /// Removes one or more <see cref="JsonConverter"/> implementations where <see cref="JsonConverter.CanConvert"/> evaluates <c>true</c> in the collection of <paramref name="converters"/>.
        /// </summary>
        /// <typeparam name="T">The type of object or value handled by the <see cref="JsonConverter"/>.</typeparam>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> RemoveAllOf<T>(this ICollection<JsonConverter> converters)
        {
            return RemoveAllOf(converters, typeof(T));
        }

        /// <summary>
        /// Removes one or more <see cref="JsonConverter"/> implementations where <see cref="JsonConverter.CanConvert"/> evaluates <c>true</c> in the collection of <paramref name="converters"/>.
        /// </summary>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <param name="types">The type of objects or values handled by a sequence of <see cref="JsonConverter"/>.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> RemoveAllOf(this ICollection<JsonConverter> converters, params Type[] types)
        {
            Validator.ThrowIfNull(converters);
            Validator.ThrowIfNull(types);
            var rejects = types.SelectMany(type => converters.Where(jc => jc.CanConvert(type))).ToList();
            foreach (var reject in rejects)
            {
                converters.Remove(reject);
            }
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="IMetadataDictionary"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddMetadataDictionaryConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new MetadataDictionaryConverter());
            return converters;
        }

        /// <summary>
        /// Adds an <see cref="IMessage{T}"/> converter (or derived) to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddMessageConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new MessageConverter());
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="IRequest"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddRequestConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new RequestConverter());
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="DateTime"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddDateTimeConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new DateTimeConverter());
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="DateTimeOffset"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="JsonConverter"/> to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddDateTimeOffsetConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new DateTimeOffsetConverter());
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="SingleValueObject{T}"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddSingleValueObjectConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new SingleValueObjectConverter());
            return converters;
        }
    }
}
