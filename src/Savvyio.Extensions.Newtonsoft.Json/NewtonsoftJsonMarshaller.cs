using System;
using System.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    /// <summary>
    /// Provides a class for marshalling data using the Newtonsoft JSON library.
    /// </summary>
    /// <seealso cref="IMarshaller" />
    public class NewtonsoftJsonMarshaller : IMarshaller
    {
        private readonly Action<NewtonsoftJsonFormatterOptions> _setup;
        
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
