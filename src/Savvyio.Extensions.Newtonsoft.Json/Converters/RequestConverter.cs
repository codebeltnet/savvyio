using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Cuemon.Extensions.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IRequest"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class RequestConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestConverter"/> class.
        /// </summary>
        public RequestConverter()
        {
        }

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <exception cref="NotImplementedException">
		/// This method is not implemented and should not be used.
		/// </exception>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
		/// </summary>
		/// <value><c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.</value>
		public override bool CanWrite { get; } = false;


		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		/// <exception cref="NotSupportedException">
		/// This deserializer only supports rehydration of <see cref="IRequest"/> implementations that either use auto-properties or have a naming convention that makes it possible to tie non-writable properties with the backing field equivalent.
		/// </exception>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var document = JObject.Load(reader);
            var instance = RuntimeHelpers.GetUninitializedObject(objectType);
            var properties = objectType.GetAllProperties();
            var jProperties = document.Properties().ToList();
            foreach (var property in properties.Where(pi => jProperties.Exists(jp => jp.Name.Equals(pi.Name, StringComparison.OrdinalIgnoreCase))))
            {
                var jProperty = jProperties.Single(jp => jp.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                if (document.Root[jProperty.Name] != null)
                {
                    var value = serializer.Deserialize(document.Root[jProperty.Name].CreateReader(), property.PropertyType);
                    if (property.CanWrite)
                    {
                        property.SetValue(instance, value);
                    }
                    else
                    {
                        var field = property.IsAutoProperty()
                            ? objectType.GetAllFields().SingleOrDefault(fi => fi.Name.StartsWith($"<{property.Name}>"))
                            : objectType.GetAllFields().SingleOrDefault(fi => fi.Name.Equals($"_{property.Name}>", StringComparison.OrdinalIgnoreCase));
                        if (field != null)
                        {
                            field.SetValue(instance, value);
                        }
                        else
                        {
                            throw new NotSupportedException($"This deserializer only supports rehydration of {nameof(IRequest)} implementations that either use auto-properties or have a naming convention that makes it possible to tie non-writable properties with the backing field equivalent.");
                        }
                    }
                }
            }
            return instance as IRequest;

        }

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
		public override bool CanConvert(Type objectType)
        {
            return typeof(IRequest).IsAssignableFrom(objectType);
        }
    }
}
