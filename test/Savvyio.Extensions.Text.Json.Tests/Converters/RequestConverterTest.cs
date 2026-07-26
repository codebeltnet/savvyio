using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Xunit;

namespace Savvyio.Extensions.Text.Json.Converters
{
    public class RequestConverterTest : Test
    {
        public RequestConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void RequestConverter_ShouldConvertOnlyRequests()
        {
            var sut = new RequestConverter();

            Assert.True(sut.CanConvert(typeof(CreateMemberCommand)));
            Assert.False(sut.CanConvert(typeof(string)));
        }

        [Fact]
        public void RequestConverter_ShouldRehydrateAutoPropertyRequests()
        {
            var sut = JsonSerializer.Deserialize<CreateMemberCommand>("{\"name\":\"Jane Doe\",\"age\":21,\"emailAddress\":\"jd@office.com\"}", CreateOptions());

            Assert.NotNull(sut);
            Assert.Equal("Jane Doe", sut.Name);
            Assert.Equal((byte)21, sut.Age);
            Assert.Equal("jd@office.com", sut.EmailAddress);
        }

        [Fact]
        public void RequestConverter_ShouldRehydrateWritableProperties()
        {
            var sut = JsonSerializer.Deserialize<WritableRequest>("{\"name\":\"Jane Doe\"}", CreateOptions());

            Assert.NotNull(sut);
            Assert.Equal("Jane Doe", sut.Name);
        }

        [Fact]
        public void RequestConverter_ShouldFailWhenNoSupportedBackingFieldExists()
        {
            var ex = Assert.Throws<NotSupportedException>(() => JsonSerializer.Deserialize<UnsupportedRequest>("{\"name\":\"Jane Doe\"}", CreateOptions()));

            Assert.StartsWith("This deserializer only supports rehydration", ex.Message);
        }

        [Fact]
        public void RequestConverter_ShouldRoundtripThroughWrite()
        {
            var options = CreateOptions();
            var json = JsonSerializer.Serialize<IRequest>(new WritableRequest { Name = "Jane Doe" }, options);

            TestOutput.WriteLine(json);

            var sut = JsonSerializer.Deserialize<WritableRequest>(json, options);

            Assert.NotNull(sut);
            Assert.Equal("Jane Doe", sut.Name);
        }

        private static JsonSerializerOptions CreateOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new RequestConverter());
            return options;
        }

        private sealed class WritableRequest : IRequest
        {
            public string Name { get; set; }
        }

        private sealed class UnsupportedRequest : IRequest
        {
            private readonly string _name = string.Empty;

            public string Name => _name;
        }
    }
}
