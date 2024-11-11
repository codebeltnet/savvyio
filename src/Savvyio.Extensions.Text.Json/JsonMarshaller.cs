using System;
using System.IO;
using Cuemon.Extensions.Text.Json.Formatters;

namespace Savvyio.Extensions.Text.Json
{
    /// <summary>
    /// Provides a class for marshalling data using native JSON support in .NET.
    /// </summary>
    /// <seealso cref="IMarshaller" />
    public class JsonMarshaller : IMarshaller
    {
        private readonly Action<JsonFormatterOptions> _setup;

        /// <summary>
        /// Provides a default instance of the <see cref="JsonMarshaller"/> class optimized for messaging.
        /// </summary>
        /// <value>The default instance of the <see cref="JsonMarshaller"/> class optimized for messaging.</value>
        public static JsonMarshaller Default { get; } = new(o => o.Settings.WriteIndented = false);

        /// <summary>
        /// Creates a new instance of the <see cref="JsonMarshaller"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
        /// <returns>JsonMarshaller.</returns>
        public static JsonMarshaller Create(Action<JsonFormatterOptions> setup = null) => new(setup);

        static JsonMarshaller()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonMarshaller"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="JsonFormatterOptions"/> which may be configured.</param>
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
