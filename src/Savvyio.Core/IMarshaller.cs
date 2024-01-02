using System;
using System.IO;

namespace Savvyio
{
    /// <summary>
    /// Defines methods for serializing and deserializing objects to and from a <see cref="Stream"/>.
    /// </summary>
    public interface IMarshaller
    {
        /// <summary>
        /// Serializes the specified <paramref name="value"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The object to serialize to <see cref="Stream"/> format.</param>
        /// <returns>A <see cref="Stream"/> of the serialized <paramref name="value"/>.</returns>
        Stream Serialize<TValue>(TValue value);

        /// <summary>
        /// Serializes the specified <paramref name="value"/> to an object of <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The object to serialize to <see cref="Stream"/> format.</param>
        /// <param name="inputType">The type of the object to serialize.</param>
        /// <returns>A <see cref="Stream"/> of the serialized <paramref name="value"/>.</returns>
        Stream Serialize(object value, Type inputType);

        /// <summary>
        /// Deserializes the specified <paramref name="data"/> into an object of <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the object to return.</typeparam>
        /// <param name="data">The object from which to deserialize the object graph.</param>
        /// <returns>An object of <typeparamref name="TValue" />.</returns>
        TValue Deserialize<TValue>(Stream data);

        /// <summary>
        /// Deserializes the specified <paramref name="data" /> into an object of <paramref name="returnType"/>.
        /// </summary>
        /// <param name="data">The string from which to deserialize the object graph.</param>
        /// <param name="returnType">The type of the deserialized object.</param>
        /// <returns>An object of <paramref name="returnType"/>.</returns>
        object Deserialize(Stream data, Type returnType);
    }
}
