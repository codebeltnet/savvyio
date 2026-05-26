using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json
{
    public class JsonSerializerExtensionsTest : Test
    {
        public JsonSerializerExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void JsonSerializerExtensions_ShouldResolveKeysWithDefaultNamingStrategy()
        {
            var sut = JsonSerializer.CreateDefault();

            Assert.Equal("MemberType", sut.ResolvePropertyKeyByConvention("MemberType"));
            Assert.Equal("MemberType", sut.ResolveDictionaryKeyByConvention("MemberType"));
        }

        [Fact]
        public void JsonSerializerExtensions_ShouldResolveKeysWithCamelCaseNamingStrategy()
        {
            var sut = JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            });

            Assert.Equal("memberType", sut.ResolvePropertyKeyByConvention("MemberType"));
            Assert.Equal("MemberType", sut.ResolveDictionaryKeyByConvention("MemberType"));
        }
    }
}
