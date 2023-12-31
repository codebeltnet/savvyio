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

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool CanWrite { get; } = false;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return typeof(IRequest).IsAssignableFrom(objectType);
        }
    }
}
