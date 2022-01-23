using System;
using Cuemon;

namespace Savvyio
{
    /// <summary>
    /// Provides access to factory methods for maintaining metadata in models.
    /// </summary>
    public static class MetadataFactory
    {
        /// <summary>
        /// Gets the value associates to the <paramref name="model"/> from the item with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to retrieve.</param>
        /// <returns>A reference to the value in the <paramref name="model"/> that is identified by <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
        public static object Get<T>(T model, string key) where T : IMetadata
        {
            if (model.Metadata.TryGetValue(key, out var result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Assigns a new <paramref name="value"/> to the <paramref name="model"/> on the item with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="model">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to retrieve.</param>
        /// <param name="value">The value of the element to change.</param>
        /// <returns>A reference to <paramref name="model"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="model" /> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        /// <exception cref="ReservedKeywordException">
        /// <paramref name="key"/> is a reserved keyword.
        /// </exception>
        public static T Set<T>(T model, string key, object value) where T : IMetadata
        {
            Validator.ThrowIfNull(model, nameof(model));
            Validator.ThrowIfNull(key, nameof(key));

            if (model.Metadata.ContainsKey(key))
            {
                model.Metadata[key] = value;
            }
            else
            {
                model.Metadata.Add(key, value);
            }
            return model;
        }

        internal static T SetUnrestricted<T>(T model, string key, object value) where T : IMetadata
        {
            Validator.ThrowIfNull(model, nameof(model));
            Validator.ThrowIfNull(key, nameof(key));

            if (model.Metadata.ContainsKey(key))
            {
                model.Metadata[key] = value;
            }
            else
            {
                model.Metadata.AddUnristricted(key, value);
            }
            return model;
        }
    }
}
