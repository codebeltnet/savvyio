using System;
using System.IO;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Savvyio.Extensions.Text.Json
{
    /// <summary>
    /// An interface for serializing and deserializing JSON objects using native JSON support.
    /// </summary>
    /// <seealso cref="ISerializerContext" />
    public class JsonSerializerContext : ISerializerContext
    {
        private readonly Action<JsonFormatterOptions> _setup;

        static JsonSerializerContext()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializerContext"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonSerializerContext(Action<JsonFormatterOptions> setup = null)
        {
            _setup = setup;
        }

        /// <inheritdoc />
        public Stream Serialize<TValue>(TValue value)
        {
            return JsonFormatter.SerializeObject(value, _setup);
        }

        /// <inheritdoc />
        public Stream Serialize(object value, Type inputType)
        {
            return JsonFormatter.SerializeObject(value, inputType, _setup);
        }

        /// <inheritdoc />
        public TValue Deserialize<TValue>(Stream data)
        {
            return JsonFormatter.DeserializeObject<TValue>(data, _setup);
        }

        /// <inheritdoc />
        public object Deserialize(Stream data, Type returnType)
        {
            return JsonFormatter.DeserializeObject(data, returnType, _setup);
        }
    }
}
