using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon;
using Cuemon.Collections.Generic;
using Savvyio.Messaging;

namespace Savvyio.EventDriven.Messaging.CloudEvents
{
    /// <summary>
    /// Provides the base class for an implementation of the <see cref="ICloudEvent{T}"/> interface.
    /// </summary>
    public abstract record CloudEvent : Acknowledgeable
    {
        /// <summary>
        /// Gets a list of the reserved keywords.
        /// </summary>
        /// <value>A list of the reserved keywords.</value>
        protected static IReadOnlyCollection<string> ReservedKeywords { get; } = Arguments.ToEnumerableOf(nameof(Id), nameof(Source), nameof(Type), nameof(Time), nameof(Data), nameof(Specversion)).ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudEvent"/> class.
        /// </summary>
        protected CloudEvent()
        {
        }

        /// <summary>
        /// Gets the identifier of the event. When combined with <see cref="Source"/>, this enables deduplication.
        /// </summary>
        /// <value>The identifier of the event.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#id</remarks>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets the context in which an event happened. When combined with <see cref="Id"/>, this enables deduplication.
        /// </summary>
        /// <value>The context in which an event happened.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#source-1</remarks>
        public string Source { get; protected set; }


        /// <summary>
        /// Gets the value describing the type of event related to the originating occurrence.
        /// </summary>
        /// <value>The value describing the type of event related to the originating occurrence.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#type</remarks>
        public string Type { get; protected set; }
        
        /// <summary>
        /// Gets the time, expressed as the Coordinated Universal Time (UTC), of when the occurrence happened.
        /// </summary>
        /// <value>The timestamp of when the occurrence happened.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#time</remarks>
        public DateTime? Time { get; protected set; }

        /// <summary>
        /// Gets version of the CloudEvents specification which the event uses.
        /// </summary>
        /// <value>The version of the CloudEvents specification which the event uses.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#specversion</remarks>
        public string Specversion { get; protected set; }
    }

    /// <summary>
    /// Provides a default implementation of the <see cref="ICloudEvent{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
    /// <seealso cref="ICloudEvent{T}" />
    public record CloudEvent<T> : CloudEvent, ICloudEvent<T> where T : IIntegrationEvent
    {
        private readonly IDictionary<string, object> _extensionAttributes = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        private static string EnsureReservedKeywordCompliance(string key) // https://github.com/cloudevents/spec/blob/v1.0.2/cloudevents/spec.md#attribute-naming-convention
        {
            Validator.ThrowIfNullOrWhitespace(key);
            Validator.ThrowIfContainsReservedKeyword(key, ReservedKeywords);
            return key.ToLowerInvariant();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudEvent{T}"/> class.
        /// </summary>
        /// <param name="message">The message to elevate to an <see cref="ICloudEvent{T}"/> compliance.</param>
        /// <param name="specversion">The version of the CloudEvents specification which the event uses.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message"/> cannot be null.
        /// </exception>
        public CloudEvent(IMessage<T> message, string specversion = null)
        {
            Validator.ThrowIfNull(message);
            Id = message.Id;
            Source = message.Source;
            Type = message.Type;
            Time = message.Time;
            Data = message.Data;
            Specversion = specversion ?? "1.0";
        }

        /// <summary>
        /// Gets the event payload.
        /// </summary>
        /// <value>The event payload.</value>
        /// <remarks>https://github.com/cloudevents/spec/blob/main/cloudevents/spec.md#event-data</remarks>
        public T Data { get; }



        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _extensionAttributes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_extensionAttributes).GetEnumerator();
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the <see cref="CloudEvent" />.
        /// </summary>
        public void Clear()
        {
            _extensionAttributes.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return _extensionAttributes.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _extensionAttributes.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        int ICollection<KeyValuePair<string, object>>.Count => _extensionAttributes.Count;

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => _extensionAttributes.IsReadOnly;

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="CloudEvent" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(string key, object value)
        {
            var parsedKey = EnsureReservedKeywordCompliance(key);
            _extensionAttributes.Add(parsedKey, value);
        }

        /// <summary>
        /// Determines whether the <see cref="CloudEvent" /> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="CloudEvent" />.</param>
        /// <returns><see langword="true" /> if the <see cref="CloudEvent" /> contains an element with the key; otherwise, <see langword="false" />.</returns>
        public bool ContainsKey(string key)
        {
            return _extensionAttributes.ContainsKey(key);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="CloudEvent" />.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns><see langword="true" /> if the element is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="key" /> was not found in the original <see cref="CloudEvent" />.</returns>
        public bool Remove(string key)
        {
            return _extensionAttributes.Remove(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
        /// <returns><see langword="true" /> if the object that implements <see cref="CloudEvent" /> contains an element with the specified key; otherwise, <see langword="false" />.</returns>
        public bool TryGetValue(string key, out object value)
        {
            return _extensionAttributes.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public object this[string key]
        {
            get => _extensionAttributes[key];
            set
            {
                var parsedKey = EnsureReservedKeywordCompliance(key);
                _extensionAttributes[parsedKey] = value;
            }
        }

        ICollection<string> IDictionary<string, object>.Keys => _extensionAttributes.Keys;

        ICollection<object> IDictionary<string, object>.Values => _extensionAttributes.Values;
    }
}
