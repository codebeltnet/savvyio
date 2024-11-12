using System;
using System.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Newtonsoft.Json;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Provides a class for marshalling data using the Newtonsoft JSON library.
    /// </summary>
    /// <seealso cref="IMarshaller" />
    public class NewtonsoftJsonMarshaller : IMarshaller
    {
        private readonly Action<NewtonsoftJsonFormatterOptions> _setup;

        /// <summary>
        /// Provides a default instance of the <see cref="NewtonsoftJsonMarshaller"/> class optimized for messaging.
        /// </summary>
        /// <value>The default instance of the <see cref="NewtonsoftJsonMarshaller"/> class optimized for messaging.</value>
        public static NewtonsoftJsonMarshaller Default { get; } = new(o => o.Settings.Formatting = Formatting.None);

        /// <summary>
        /// Creates a new instance of the <see cref="NewtonsoftJsonMarshaller"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which may be configured.</param>
        /// <returns>JsonMarshaller.</returns>
        public static NewtonsoftJsonMarshaller Create(Action<NewtonsoftJsonFormatterOptions> setup = null) => new(setup);

        static NewtonsoftJsonMarshaller()
        {
            Bootstrapper.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewtonsoftJsonMarshaller"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="NewtonsoftJsonFormatterOptions"/> which need to be configured.</param>
        public NewtonsoftJsonMarshaller(Action<NewtonsoftJsonFormatterOptions> setup = null)
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
