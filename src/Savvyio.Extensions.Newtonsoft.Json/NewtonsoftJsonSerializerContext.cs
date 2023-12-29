using System;
using System.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// An interface for serializing and deserializing JSON objects using Newtonsoft JSON.
    /// </summary>
    /// <seealso cref="ISerializerContext" />
    public class NewtonsoftJsonSerializerContext : ISerializerContext
    {
        private readonly Action<NewtonsoftJsonFormatterOptions> _setup;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonSerializerContext"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which may be configured.</param>
        public NewtonsoftJsonSerializerContext(Action<NewtonsoftJsonFormatterOptions> setup)
        {
            _setup = setup;
        }

        /// <inheritdoc />
        public Stream Serialize<TValue>(TValue value)
        {
            return NewtonsoftJsonFormatter.SerializeObject(value, _setup);
        }

        /// <inheritdoc />
        public Stream Serialize(object value, Type inputType)
        {
            return NewtonsoftJsonFormatter.SerializeObject(value, inputType, _setup);
        }

        /// <inheritdoc />
        public TValue Deserialize<TValue>(Stream data)
        {
            return NewtonsoftJsonFormatter.DeserializeObject<TValue>(data, _setup);
        }

        /// <inheritdoc />
        public object Deserialize(Stream data, Type returnType)
        {
            return NewtonsoftJsonFormatter.DeserializeObject(data, returnType, _setup);
        }
    }
}
