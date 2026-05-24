using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Savvyio;
using Savvyio.Assets.Commands;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    public class RequestConverterTest : Test
    {
        public RequestConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RequestConverter_ShouldExposeReadOnlyCapabilities()
        {
            var sut = new RequestConverter();

            Assert.False(sut.CanWrite);
            Assert.True(sut.CanConvert(typeof(CreateMemberCommand)));
            Assert.False(sut.CanConvert(typeof(string)));
        }

        [Fact]
        public void RequestConverter_ShouldThrowWhenWritingJson()
        {
            var sut = new RequestConverter();
            using var writer = new JsonTextWriter(new StringWriter());

            Assert.Throws<NotImplementedException>(() => sut.WriteJson(writer, new CreateMemberCommand("Jane Doe", 21, "jd@office.com"), JsonSerializer.CreateDefault()));
        }

        [Fact]
        public void RequestConverter_ShouldRehydrateAutoPropertyRequests()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new RequestConverter());

            var sut = JsonConvert.DeserializeObject<CreateMemberCommand>("{\"name\":\"Jane Doe\",\"age\":21,\"emailAddress\":\"jd@office.com\"}", settings);

            Assert.NotNull(sut);
            Assert.Equal("Jane Doe", sut.Name);
            Assert.Equal((byte)21, sut.Age);
            Assert.Equal("jd@office.com", sut.EmailAddress);
        }

        [Fact]
        public void RequestConverter_ShouldFailWhenNoSupportedBackingFieldExists()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new RequestConverter());

            var ex = Assert.Throws<NotSupportedException>(() => JsonConvert.DeserializeObject<UnsupportedRequest>("{\"name\":\"Jane Doe\"}", settings));

            Assert.StartsWith("This deserializer only supports rehydration", ex.Message);
        }

        private sealed class UnsupportedRequest : IRequest
        {
            private readonly string _name = string.Empty;

            public string Name => _name;
        }
    }
}
