using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Codebelt.Extensions.Xunit;
using Savvyio.Extensions.Text.Json.Converters;
using Xunit;

namespace Savvyio.Extensions.Text.Json
{
    public class JsonConverterExtensionsTest : Test
    {
        public JsonConverterExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDateTimeConverter_ShouldAddConverter()
        {
            var converters = new List<JsonConverter>();
            converters.AddDateTimeConverter();

            Assert.Single(converters, c => c is DateTimeConverter);
        }

        [Fact]
        public void AddDateTimeOffsetConverter_ShouldAddConverter()
        {
            var converters = new List<JsonConverter>();
            converters.AddDateTimeOffsetConverter();

            Assert.Single(converters, c => c is DateTimeOffsetConverter);
        }

        [Fact]
        public void AddSingleValueObjectConverter_ShouldAddConverter()
        {
            var converters = new List<JsonConverter>();
            converters.AddSingleValueObjectConverter();

            Assert.Single(converters, c => c is SingleValueObjectConverter);
        }

        [Fact]
        public void AddMetadataDictionaryConverter_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).AddMetadataDictionaryConverter());
        }

        [Fact]
        public void AddMessageConverter_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).AddMessageConverter());
        }

        [Fact]
        public void AddRequestConverter_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).AddRequestConverter());
        }

        [Fact]
        public void AddDateTimeConverter_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).AddDateTimeConverter());
        }

        [Fact]
        public void AddDateTimeOffsetConverter_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).AddDateTimeOffsetConverter());
        }

        [Fact]
        public void AddSingleValueObjectConverter_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).AddSingleValueObjectConverter());
        }

        [Fact]
        public void RemoveAllOf_Generic_ShouldRemoveMatchingConverter()
        {
            var converters = new List<JsonConverter>();
            converters.AddDateTimeConverter();
            converters.AddDateTimeOffsetConverter();

            Assert.Equal(2, converters.Count);

            converters.RemoveAllOf<DateTime>();

            Assert.DoesNotContain(converters, c => c is DateTimeConverter);
        }

        [Fact]
        public void RemoveAllOf_Generic_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).RemoveAllOf<DateTime>());
        }

        [Fact]
        public void RemoveAllOf_Params_ShouldRemoveMatchingConverters()
        {
            var converters = new List<JsonConverter>();
            converters.AddDateTimeConverter();
            converters.AddDateTimeOffsetConverter();

            converters.RemoveAllOf(typeof(DateTime), typeof(DateTimeOffset));

            Assert.Empty(converters);
        }

        [Fact]
        public void RemoveAllOf_Params_ShouldThrowArgumentNullException_WhenConvertersIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((ICollection<JsonConverter>)null).RemoveAllOf(typeof(DateTime)));
        }
    }
}
