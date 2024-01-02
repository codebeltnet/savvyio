using System;
using System.IO;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Savvyio.Extensions.Text.Json
{
    /// <summary>
    /// Provides a class for marshalling data using native JSON support from .NET.
    /// </summary>
    /// <seealso cref="IMarshaller" />
    public class JsonMarshaller : IMarshaller
    {
        private readonly Action<JsonFormatterOptions> _setup;

        static JsonMarshaller()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMarshaller"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which need to be configured.</param>
        public JsonMarshaller(Action<JsonFormatterOptions> setup = null)
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
