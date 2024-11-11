using System;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Text.Json.Domain
{
    public class DomainTest : Test
    {
        public DomainTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Coordinates_ShouldBeAbleToBothSerializeAndDeserialize()
        {
            var sut = new Coordinates(10.1, 10.2);

            var json = new JsonMarshaller().Serialize(sut);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var rehydrated = new JsonMarshaller().Deserialize<Coordinates>(json);

            Assert.Equivalent(sut, rehydrated, true);
        }

        [Fact]
        public void Timestamp_ShouldBeAbleToBothSerializeAndDeserialize()
        {
            var sut = new Timestamp(DateTimeOffset.UtcNow)
            {
                IsDaylightSavingTime = false,
                TimeZone = "Europe/Berlin"
            };

            var json = new JsonMarshaller().Serialize(sut);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var rehydrated = new JsonMarshaller().Deserialize<Timestamp>(json);

            Assert.Equivalent(sut, rehydrated, true);
        }

        [Fact]
        public void PlatformProviderId_ShouldBeAbleToBothSerializeAndDeserialize()
        {
            var sut = new PlatformProviderId(Guid.NewGuid());

            var json = new JsonMarshaller().Serialize(sut);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var rehydrated = new JsonMarshaller().Deserialize<PlatformProviderId>(json);

            Assert.Equivalent(sut, rehydrated, true);
        }
    }
}
