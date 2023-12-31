using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Cuemon.Extensions.Reflection;

namespace Savvyio.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IRequest"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    public class RequestConverter : JsonConverter<IRequest>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IRequest).IsAssignableFrom(typeToConvert);
        }

        /// <inheritdoc />
        public override IRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var instance = RuntimeHelpers.GetUninitializedObject(typeToConvert);
                var properties = typeToConvert.GetAllProperties();
                var jProperties = document.RootElement.EnumerateObject();
                foreach (var property in properties.Where(pi => jProperties.Any(jp => jp.Name.Equals(pi.Name, StringComparison.OrdinalIgnoreCase))))
                {
                    var jProperty = jProperties.Single(jp => jp.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                    if (document.RootElement.TryGetProperty(jProperty.Name, out var element))
                    {
                        var value = element.Deserialize(property.PropertyType, options);
                        if (property.CanWrite)
                        {
                            property.SetValue(instance, value);
                        }
                        else
                        {
                            var field = typeToConvert.GetAllFields().SingleOrDefault(fi => fi.Name.Contains(property.Name));
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
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, IRequest value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options.Clone(jso => jso.Converters.RemoveAllOf<IRequest>())); // prevent stackoverflow in case this method gets called
        }
    }
}
