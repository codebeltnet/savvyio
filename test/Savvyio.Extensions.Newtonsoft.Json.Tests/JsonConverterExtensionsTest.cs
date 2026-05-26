using System;
using System.Collections.Generic;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json.Converters;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    public class JsonConverterExtensionsTest : Test
    {
        public JsonConverterExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void JsonConverterExtensions_ShouldAddExpectedConverters()
        {
            ICollection<JsonConverter> sut = new List<JsonConverter>();

            var chained = sut.AddValueObjectConverter()
                .AddAggregateRootConverter<Guid>()
                .AddMetadataDictionaryConverter()
                .AddRequestConverter()
                .AddMessageConverter()
                .AddSingleValueObjectConverter();

            Assert.Same(sut, chained);
            Assert.Contains(sut, c => c is ValueObjectConverter);
            Assert.Contains(sut, c => c is AggregateRootConverter<Guid>);
            Assert.Contains(sut, c => c is RequestConverter);
            Assert.Contains(sut, c => c is MessageConverter);
            Assert.Contains(sut, c => c is SingleValueObjectConverter);
            Assert.Equal(6, sut.Count);
        }

        [Fact]
        public void AddMetadataDictionaryConverter_ShouldDeserializeMetadataDictionary()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.AddMetadataDictionaryConverter();

            var sut = JsonConvert.DeserializeObject<IMetadataDictionary>("{\"memberType\":\"custom\",\"attempts\":42}", settings);

            Assert.IsType<MetadataDictionary>(sut);
            Assert.Equal("custom", sut["memberType"]);
            Assert.Equal(42L, sut["attempts"]);
        }
    }
}
