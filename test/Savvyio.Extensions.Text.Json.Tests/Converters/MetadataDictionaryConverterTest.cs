using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters
{
    public class MetadataDictionaryConverterTest : Test
    {
        public MetadataDictionaryConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CanConvert_ShouldReturnTrue_ForIMetadataDictionary()
        {
            var converter = new MetadataDictionaryConverter();

            Assert.True(converter.CanConvert(typeof(IMetadataDictionary)));
            Assert.True(converter.CanConvert(typeof(MetadataDictionary)));
        }

        [Fact]
        public void CanConvert_ShouldReturnFalse_ForUnrelatedTypes()
        {
            var converter = new MetadataDictionaryConverter();

            Assert.False(converter.CanConvert(typeof(string)));
            Assert.False(converter.CanConvert(typeof(int)));
        }

        [Fact]
        public void Read_ShouldHandleBooleanFalse()
        {
            var json = """{"flag": false}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            Assert.Equal(false, result["flag"]);
        }

        [Fact]
        public void Read_ShouldHandleBooleanTrue()
        {
            var json = """{"flag": true}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            Assert.Equal(true, result["flag"]);
        }

        [Fact]
        public void Read_ShouldHandleNullValue()
        {
            var json = """{"key": null}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            Assert.Null(result["key"]);
        }

        [Fact]
        public void Read_ShouldHandleInt64Number()
        {
            var json = """{"count": 9876543210}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            Assert.Equal(9876543210L, result["count"]);
        }

        [Fact]
        public void Read_ShouldHandleDecimalNumber()
        {
            var json = """{"price": 3.14}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            Assert.Equal(3.14m, result["price"]);
        }

        [Fact]
        public void Read_ShouldHandleNestedObject()
        {
            var json = """{"outer": {"inner": "value"}}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            var nested = Assert.IsAssignableFrom<IMetadataDictionary>(result["outer"]);
            Assert.Equal("value", nested["inner"]);
        }

        [Fact]
        public void Read_ShouldHandleArrayValue()
        {
            var json = """{"items": ["a", "b", "c"]}""";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            var list = Assert.IsAssignableFrom<List<object>>(result["items"]);
            Assert.Equal(3, list.Count);
            Assert.Equal("a", list[0]);
            Assert.Equal("b", list[1]);
            Assert.Equal("c", list[2]);

            TestOutput.WriteLine(string.Join(", ", list));
        }

        [Fact]
        public void Read_ShouldDeserializeDateTimeString()
        {
            var dt = new DateTime(2023, 11, 16, 23, 24, 17, DateTimeKind.Utc);
            var json = $"{{\"ts\": \"{dt:O}\"}}";
            var options = CreateOptions();

            var result = JsonSerializer.Deserialize<IMetadataDictionary>(json, options);

            Assert.NotNull(result);
            Assert.IsType<DateTime>(result["ts"]);
            Assert.Equal(dt, (DateTime)result["ts"]);

            TestOutput.WriteLine(result["ts"].ToString());
        }

        [Fact]
        public void Write_ShouldSerializeMetadataDictionary()
        {
            var md = new MetadataDictionary
            {
                { "key1", "value1" },
                { "key2", 99L },
                { "key3", false }
            };

            var options = CreateOptions();
            var json = JsonSerializer.Serialize<IMetadataDictionary>(md, options);

            TestOutput.WriteLine(json);

            Assert.Contains("key1", json);
            Assert.Contains("value1", json);
            Assert.Contains("key2", json);
            Assert.Contains("99", json);
            Assert.Contains("key3", json);
            Assert.Contains("false", json);
        }

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new MetadataDictionaryConverter());
            return options;
        }
    }
}
