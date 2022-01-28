using System;
using System.Collections;
using System.Collections.Generic;
using Cuemon;
using Cuemon.Collections.Generic;
using Cuemon.Extensions;

namespace Savvyio
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IMetadataDictionary"/> interface. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="IMetadataDictionary" />
    public sealed class MetadataDictionary : IMetadataDictionary
    {
        private static readonly IEnumerable<string> ReservedKeywords = Arguments.ToEnumerableOf(Timestamp, MemberType, CorrelationId, CausationId);
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>();

        /// <summary>
        /// Timestamp is a reserved keyword.
        /// </summary>
        public const string Timestamp = nameof(Timestamp);

        /// <summary>
        /// MemberType is a reserved keyword.
        /// </summary>
        public const string MemberType = nameof(MemberType);

        /// <summary>
        /// CorrelationId is a reserved keyword.
        /// </summary>
        public const string CorrelationId = nameof(CorrelationId);

        /// <summary>
        /// CausationId is a reserved keyword.
        /// </summary>
        public const string CausationId = nameof(CausationId);

        /// <summary>
        /// EventId is a reserved keyword.
        /// </summary>
        public const string EventId = nameof(EventId);

        /// <summary>
        /// AggregateVersion is a reserved keyword.
        /// </summary>
        public const string AggregateVersion = nameof(AggregateVersion);

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataDictionary"/> class.
        /// </summary>
        public MetadataDictionary()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public object this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => _dictionary.IsReadOnly;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys => _dictionary.Keys;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<object> Values => _dictionary.Values;

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(string key, object value)
        {
            Validator.ThrowIf.ContainsReservedKeyword(key, ReservedKeywords, nameof(key), FormattableString.Invariant($"Unable to add the specified {nameof(key)} as it is a reserved keyword."));
            _dictionary.Add(key, value);
        }

        IMetadataDictionary IMetadataDictionary.AddUnrestricted(string key, object value)
        {
            _dictionary.Add(key, value);
            return this;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />.</returns>
        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="MetadataDictionary" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="MetadataDictionary" />.</param>
        /// <returns><see langword="true" /> if the <see cref="MetadataDictionary" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="MetadataDictionary" />.</returns>
        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }
        
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="MetadataDictionary" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }
    }
}
