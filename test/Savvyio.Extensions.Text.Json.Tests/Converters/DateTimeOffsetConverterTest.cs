using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters
{
    public class DateTimeOffsetConverterTest : Test
    {
        public DateTimeOffsetConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanConvert_ReturnsTrue_ForDateTime()
        {
            var converter = new DateTimeOffsetConverter();
            // Note: CanConvert checks for DateTime (not DateTimeOffset) by design in this implementation
            Assert.True(converter.CanConvert(typeof(DateTime)));
        }

        [Fact]
        public void Write_ShouldSerializeDateTimeOffset_AsIso8601()
        {
            var dto = new DateTimeOffset(2023, 11, 16, 23, 24, 17, TimeSpan.Zero);
            var converter = new DateTimeOffsetConverter();
            var buffer = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(buffer);

            converter.Write(writer, dto, new JsonSerializerOptions());
            writer.Flush();

            var json = Encoding.UTF8.GetString(buffer.WrittenSpan);
            TestOutput.WriteLine(json);

            Assert.Contains("2023-11-16", json);
        }

        [Fact]
        public void Read_ShouldDeserializeDateTimeOffset_FromIso8601()
        {
            var dto = new DateTimeOffset(2023, 11, 16, 23, 24, 17, TimeSpan.Zero);
            var converter = new DateTimeOffsetConverter();
            var jsonString = $"\"{dto:O}\"";
            var bytes = Encoding.UTF8.GetBytes(jsonString);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();

            var result = converter.Read(ref reader, typeof(DateTimeOffset), new JsonSerializerOptions());

            Assert.Equal(dto, result);
        }
    }
}
