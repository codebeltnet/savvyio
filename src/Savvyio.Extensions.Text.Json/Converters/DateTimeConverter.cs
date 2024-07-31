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
	    /// <summary>
	    /// Determines whether the specified type can be converted.
	    /// </summary>
	    /// <param name="typeToConvert">The type to compare against.</param>
	    /// <returns><see langword="true" /> if the type can be converted; otherwise, <see langword="false" />.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(DateTime);
        }

	    /// <summary>
	    /// Reads and converts the JSON to <paramref name="typeToConvert"/>.
	    /// </summary>
	    /// <param name="reader">The reader.</param>
	    /// <param name="typeToConvert">The type to convert.</param>
	    /// <param name="options">An object that specifies serialization options to use.</param>
	    /// <returns>The converted value.</returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetDateTime();
        }

	    /// <summary>
	    /// Writes a specified <paramref name="value"/> as JSON.
	    /// </summary>
	    /// <param name="writer">The writer to write to.</param>
	    /// <param name="value">The value to convert to JSON.</param>
	    /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}
