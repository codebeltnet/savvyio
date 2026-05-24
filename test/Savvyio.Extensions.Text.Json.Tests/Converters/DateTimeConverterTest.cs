using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters
{
    public class DateTimeConverterTest : Test
    {
        public DateTimeConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanConvert_ShouldReturnTrue_ForDateTime()
        {
            var converter = new DateTimeConverter();
            Assert.True(converter.CanConvert(typeof(DateTime)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalse_ForOtherTypes()
        {
            var converter = new DateTimeConverter();
            Assert.False(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(DateTimeOffset)));
        }

        [Fact]
        public void Write_ShouldSerializeDateTime_AsIso8601()
        {
            var utc = new DateTime(2023, 11, 16, 23, 24, 17, DateTimeKind.Utc);
            var converter = new DateTimeConverter();
            var buffer = new ArrayBufferWriter<byte>();
            using var writer = new Utf8JsonWriter(buffer);

            converter.Write(writer, utc, new JsonSerializerOptions());
            writer.Flush();

            var json = Encoding.UTF8.GetString(buffer.WrittenSpan);
            TestOutput.WriteLine(json);

            Assert.Contains("2023-11-16", json);
        }

        [Fact]
        public void Read_ShouldDeserializeDateTime_FromIso8601()
        {
            var utc = new DateTime(2023, 11, 16, 23, 24, 17, DateTimeKind.Utc);
            var converter = new DateTimeConverter();
            var jsonString = $"\"{utc:O}\"";
            var bytes = Encoding.UTF8.GetBytes(jsonString);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();

            var result = converter.Read(ref reader, typeof(DateTime), new JsonSerializerOptions());

            Assert.Equal(utc, result);
        }
    }
}
