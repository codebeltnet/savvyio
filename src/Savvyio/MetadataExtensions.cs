using System;
using Cuemon;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="IMetadata"/> interface.
    /// </summary>
    public static class MetadataExtensions
    {
        /// <summary>
        /// Add or update a set of metadata to the <paramref name="model"/>.
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
        /// Copies metadata from the <paramref name="source"/> to the <paramref name="model"/> if not already existing.
        /// </summary>
        /// <typeparam name="TSource">The giving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <typeparam name="TDestination">The receiving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend. Receives metadata from <paramref name="source"/>.</param>
        /// <param name="source">The model that will give metata to <paramref name="model"/>.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        public static TDestination MergeMetadata<TSource, TDestination>(this TDestination model, TSource source) 
            where TSource : IMetadata
            where TDestination : IMetadata
        {
            if (model != null && source != null)
            {
                foreach (var entry in source.Metadata)
                {
                    if (!model.Metadata.ContainsKey(entry.Key))
                    {
                        model.Metadata.AddUnristricted(entry.Key, entry.Value); // bypass reserved keyword check since the value is only added if non-existing
                    }
                }
            }
            return model;
        }
    }
}
