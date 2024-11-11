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
        /// <summary>
        /// Determines whether the specified type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type to compare against.</param>
        /// <returns><see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IRequest).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Reads and converts the JSON to <paramref name="typeToConvert"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        /// <exception cref="NotSupportedException">
        /// This deserializer only supports rehydration of <see cref="IRequest"/> implementations that either use auto-properties or have a naming convention that makes it possible to tie non-writable properties with the backing field equivalent.
        /// </exception>
        public override IRequest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var document = JsonDocument.ParseValue(ref reader))
            {
                var instance = RuntimeHelpers.GetUninitializedObject(typeToConvert);
                var properties = typeToConvert.GetAllProperties();
                var jProperties = document.RootElement.EnumerateObject();
                foreach (var property in properties.Where(pi => jProperties.Any(jp => jp.Name.Equals(options.PropertyNamingPolicy!.ConvertName(pi.Name), StringComparison.OrdinalIgnoreCase))))
                {
                    var jProperty = jProperties.Single(jp => jp.Name.Equals(options.PropertyNamingPolicy!.ConvertName(property.Name), StringComparison.OrdinalIgnoreCase));
                    if (document.RootElement.TryGetProperty(jProperty.Name, out var element))
                    {
                        var value = element.Deserialize(property.PropertyType, options);
                        if (property.CanWrite)
                        {
                            property.SetValue(instance, value);
                        }
                        else
                        {
                            var field = property.IsAutoProperty()
                                ? typeToConvert.GetAllFields().SingleOrDefault(fi => fi.Name.StartsWith($"<{property.Name}>"))
                                : typeToConvert.GetAllFields().SingleOrDefault(fi => fi.Name.Equals($"_{property.Name}>", StringComparison.OrdinalIgnoreCase));
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

        /// <summary>
        /// Writes a specified <paramref name="value"/> as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, IRequest value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options.Clone(jso => jso.Converters.RemoveAllOf<IRequest>())); // prevent stackoverflow in case this method gets called
        }
    }
}
