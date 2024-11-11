using System;
using System.Linq;
using System.Reflection;
using Cuemon.Extensions;
using Codebelt.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Reflection;
using Newtonsoft.Json;
using Savvyio.Domain;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="SingleValueObject{T}"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class SingleValueObjectConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleValueObjectConverter"/> class.
        /// </summary>
        public SingleValueObjectConverter()
        {
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) { return; }
            var objectType = value.GetType();
            var valueType = objectType.GetInheritedTypes().Single(type => type.IsGenericType
                                                                          && type.GetGenericTypeDefinition() == typeof(SingleValueObject<>)).GetGenericArguments()[0];
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(SingleValueObjectConverter<>).MakeGenericType(valueType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;
            converter.WriteJson(writer, value, serializer);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var valueType = objectType.GetInheritedTypes().Single(type => type.IsGenericType
                                                                          && type.GetGenericTypeDefinition() == typeof(SingleValueObject<>)).GetGenericArguments()[0];
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(SingleValueObjectConverter<>).MakeGenericType(valueType),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;
            return converter.ReadJson(reader, objectType, existingValue, serializer);
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.HasTypes(typeof(SingleValueObject<>));
        }
    }

    internal class SingleValueObjectConverter<T> : JsonConverter<SingleValueObject<T>>
    {
        public SingleValueObjectConverter()
        {
        }

        public override void WriteJson(JsonWriter writer, SingleValueObject<T> value, JsonSerializer serializer)
        {
            writer.WriteObject(value.Value, serializer);
        }

        public override SingleValueObject<T> ReadJson(JsonReader reader, Type objectType, SingleValueObject<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var valueType = typeof(T);
            return Activator.CreateInstance(objectType, serializer.Deserialize(reader, valueType)) as SingleValueObject<T>;
        }
    }
}
