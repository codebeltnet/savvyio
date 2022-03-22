using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Collections.Generic;

namespace Savvyio
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IMetadataDictionary"/> interface. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="IMetadataDictionary" />
    public sealed class MetadataDictionary : IMetadataDictionary
    {
        private readonly IDictionary<string, object> _dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets a list of the reserved keywords.
        /// </summary>
        /// <value>A list of the reserved keywords.</value>
        public static IReadOnlyCollection<string> ReservedKeywords { get; } = Arguments.ToEnumerableOf(Timestamp, MemberType, CorrelationId, CausationId, EventId, AggregateVersion).ToList();

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
        /// Initializes a new instance of the <see cref="MetadataDictionary" /> class.
        /// </summary>
        public MetadataDictionary()
        {
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public object this[string key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <value>The number of key/value pairs contained in the <see cref="MetadataDictionary"/>.</value>
        public int Count => _dictionary.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="MetadataDictionary" />is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => _dictionary.IsReadOnly;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <value>A <see cref="Dictionary{TKey,TValue}.KeyCollection"/> containing the keys in the <see cref="MetadataDictionary"/>.</value>
        public ICollection<string> Keys => _dictionary.Keys;

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <value>A <see cref="Dictionary{TKey,TValue}.ValueCollection"/> containing the values in the <see cref="MetadataDictionary"/>.</value>
        public ICollection<object> Values => _dictionary.Values;

        /// <summary>
        /// Adds an item to the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="MetadataDictionary" />.</param>
        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="ReservedKeywordException">
        /// The specified <paramref name="key"/> is a reserved keyword.
        /// </exception>
        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        /// <summary>
        /// Removes all items from the <see cref="MetadataDictionary" />.
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// Copies the elements of the <see cref="MetadataDictionary" /> to an array of type <see cref="KeyValuePair{TKey,TValue}"/>, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="MetadataDictionary" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines whether the <see cref="MetadataDictionary" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="MetadataDictionary" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> is found in the <see cref="MetadataDictionary" />; otherwise, <see langword="false" />.</returns>
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
        /// Removes the first occurrence of a specific object from the <see cref="MetadataDictionary" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="MetadataDictionary" />.</param>
        /// <returns><see langword="true" /> if <paramref name="item" /> was successfully removed from the <see cref="MetadataDictionary" />; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="item" /> is not found in the original <see cref="MetadataDictionary" />.</returns>
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
