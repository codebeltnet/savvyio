using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Text.Json;
using Savvyio.Domain;

namespace Savvyio.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="SingleValueObject{T}"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class SingleValueObjectConverter : JsonConverterFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleValueObjectConverter"/> class.
        /// </summary>
        public SingleValueObjectConverter()
        {
        }

        /// <summary>
        /// When overridden in a derived class, determines whether the converter instance can convert the specified object type.
        /// </summary>
        /// <param name="typeToConvert">The type of the object to check whether it can be converted by this converter instance.</param>
        /// <returns><see langword="true" /> if the instance can convert the specified object type; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.HasTypes(typeof(SingleValueObject<>));
        }

        /// <summary>
        /// Creates a converter for a specified type.
        /// </summary>
        /// <param name="typeToConvert">The type handled by the converter.</param>
        /// <param name="options">The serialization options to use.</param>
        /// <returns>A converter that is compatible with <paramref name="typeToConvert" />.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var valueType = typeToConvert.GetInheritedTypes().Single(type => type.IsGenericType
                                                                               && type.GetGenericTypeDefinition() == typeof(SingleValueObject<>)).GetGenericArguments()[0];
            return (JsonConverter)Activator.CreateInstance(
                typeof(SingleValueObjectConverter<>).MakeGenericType(valueType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;
        }
    }

    internal class SingleValueObjectConverter<T> : JsonConverter<SingleValueObject<T>>
    {
        public SingleValueObjectConverter()
        {
        }


        public override SingleValueObject<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var valueType = typeof(T);
                return Activator.CreateInstance(typeToConvert, document.RootElement.Deserialize(valueType)) as SingleValueObject<T>;
            }
        }

        public override void Write(Utf8JsonWriter writer, SingleValueObject<T> value, JsonSerializerOptions options)
        {
            writer.WriteObject(value.Value, options);
        }
    }
}
