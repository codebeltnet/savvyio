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
        /// Gets the value associates to the <paramref name="request"/> from the item with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to retrieve.</param>
        /// <returns>A reference to the value in the <paramref name="request"/> that is identified by <paramref name="key"/>, if the entry exists; otherwise, <c>null</c>.</returns>
        public static object Get<T>(T request, string key) where T : IMetadata
        {
            if (request.Metadata.TryGetValue(key, out var result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Assigns a new <paramref name="value"/> to the <paramref name="request"/> on the item with the specified <paramref name="key"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="IMetadata"/> interface.</typeparam>
        /// <param name="request">The <see cref="IMetadata"/> to extend.</param>
        /// <param name="key">The key of the element to retrieve.</param>
        /// <param name="value">The value of the element to change.</param>
        /// <returns>A reference to <paramref name="request"/> after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request" /> cannot be null -or-
        /// <paramref name="key"/> cannot be null.
        /// </exception>
        /// <exception cref="ReservedKeywordException">
        /// <paramref name="key"/> is a reserved keyword.
        /// </exception>
        public static T Set<T>(T request, string key, object value) where T : IMetadata
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(key, nameof(key));

            if (request.Metadata.ContainsKey(key))
            {
                request.Metadata[key] = value;
            }
            else
            {
                request.Metadata.Add(key, value);
            }
            return request;
        }
    }
}
