using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Savvyio.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts an <see cref="IMetadataDictionary"/> to or from JSON.
    /// </summary>
    /// <seealso cref="JsonConverter" />
    /// <remarks>Inspiration taken from this article: https://josef.codes/custom-dictionary-string-object-jsonconverter-for-system-text-json/</remarks>
    public class MetadataDictionaryConverter : JsonConverter<IMetadataDictionary>
    {
        /// <summary>
        /// Determines whether the specified type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type to compare against.</param>
        /// <returns><see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IMetadataDictionary).IsAssignableFrom(typeToConvert);
        }

        /// <summary>
        /// Reads and converts the JSON to  <paramref name="typeToConvert"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override IMetadataDictionary Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var md = new MetadataDictionary();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) { return md; }
                var propertyName = reader.GetString();
                reader.Read();
                md.Add(propertyName!, ExtractValue(ref reader, options));
            }
            return md;
        }

        /// <summary>
        /// Writes a specified <paramref name="value"/> as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, IMetadataDictionary value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options.Clone(jso => jso.Converters.RemoveAllOf<IMetadataDictionary>())); // prevent stackoverflow in case this method gets called
        }

        private object ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    if (reader.TryGetDateTime(out var date))
                    {
                        return date;
                    }
                    return reader.GetString();
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out var result))
                    {
                        return result;
                    }
                    return reader.GetDecimal();
                case JsonTokenType.StartObject:
                    return Read(ref reader, null!, options);
                case JsonTokenType.StartArray:
                    var list = new List<object>();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        list.Add(ExtractValue(ref reader, options));
                    }
                    return list;
                default:
                    throw new JsonException($"'{reader.TokenType}' is not supported.");
            }
        }
    }
}
