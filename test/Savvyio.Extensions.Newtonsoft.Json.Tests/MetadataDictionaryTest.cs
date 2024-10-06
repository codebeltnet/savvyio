using System;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Newtonsoft.Json.Formatters;
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    public class MetadataDictionaryTest : Test
    {
        public MetadataDictionaryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void MetadataDictionary_ShouldUseCustomConverter()
        {
            var md = new MetadataDictionary()
            {
                { "guid", $"{Guid.NewGuid()}" },
                { "int32", 2312313 },
                { "dateTime", DateTime.UtcNow }
            };

            var json = NewtonsoftJsonFormatter.SerializeObject(md);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var mdRehydrated = NewtonsoftJsonFormatter.DeserializeObject<IMetadataDictionary>(json, o =>
            {
                o.Settings.Converters
                    .AddMessageConverter()
                    .AddMetadataDictionaryConverter();
            });

            Assert.Equivalent(md, mdRehydrated, true);
        }
    }
}
