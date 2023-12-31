using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Savvyio.Extensions.Text.Json.Converters
{
    /// <summary>
    /// Converts a <see cref="DateTime"/> to or from JSON using ISO8601.
    /// </summary>
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

        /// <inheritdoc />
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime();
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}
