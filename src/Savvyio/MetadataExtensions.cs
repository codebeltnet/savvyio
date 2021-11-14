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
        /// Adds a new set of metadata to the <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        /// <exception cref="ReservedKeywordException">
        /// <paramref name="key"/> is a reserved keyword.
        /// </exception>
        public static T AddOrUpdateMetadata<T>(this T model, string key, object value) where T : IMetadata
        {
            MetadataFactory.Set(model, key, value);
            return model;
        }

        /// <summary>
        /// Copies the metadata from the <paramref name="giver"/> to the <paramref name="recipient"/> if not already existing.
        /// </summary>
        /// <typeparam name="TSource">The giving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <typeparam name="TDestination">The receiving type of the model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="recipient">The model that will receive metadata from <paramref name="giver"/>.</param>
        /// <param name="giver">The model that will give metata to <paramref name="recipient"/>.</param>
        /// <returns>A reference to <paramref name="recipient"/> after the operation has completed.</returns>
        public static TDestination TakeMetadata<TSource, TDestination>(this TDestination recipient, TSource giver) 
            where TSource : IMetadata
            where TDestination : IMetadata
        {
            foreach (var entry in giver.Metadata)
            {
                if (!recipient.Metadata.ContainsKey(entry.Key))
                {
                    recipient.Metadata.AddUnristricted(entry.Key, entry.Value); // bypass reserved keyword check since the value is only added if non-existing
                }
            }
            return recipient;
        }
    }
}
