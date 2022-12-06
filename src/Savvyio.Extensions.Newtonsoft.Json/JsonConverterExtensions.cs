using System;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Reflection;
using Newtonsoft.Json;
using Savvyio.Domain;
using Savvyio.Extensions.Newtonsoft.Json.Converters;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Extension methods for the <see cref="JsonConverter"/> class.
    /// </summary>
    public static class JsonConverterExtensions
    {
        /// <summary>
        /// Adds a <see cref="ValueObject"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection to extend.</param>
        /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddValueObjectConverter(this ICollection<JsonConverter> converters, Action<ActivatorOptions> setup = null)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new ValueObjectConverter(setup));
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="AggregateRoot{TKey}"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection to extend.</param>
        /// <param name="setup">The <see cref="ActivatorOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddAggregateRootConverter<TKey>(this ICollection<JsonConverter> converters, Action<ActivatorOptions> setup = null)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(new AggregateRootConverter<TKey>());
            return converters;
        }

        /// <summary>
        /// Adds a <see cref="IMetadataDictionary"/> converter to the collection.
        /// </summary>
        /// <param name="converters">The collection to extend.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converters"/> cannot be null.
        /// </exception>
        public static ICollection<JsonConverter> AddMetadataDictionaryConverter(this ICollection<JsonConverter> converters)
        {
            Validator.ThrowIfNull(converters);
            converters.Add(DynamicJsonConverter.Create<IMetadataDictionary>(null, (reader, _, _, _) =>
            {
                var md = new MetadataDictionary();
                var result = JData.ReadAll(reader);
                foreach (var entry in result)
                {
                    md.Add(entry.PropertyName, entry.Value);
                }
                return md;
            }));
            return converters;
        }
    }
}
