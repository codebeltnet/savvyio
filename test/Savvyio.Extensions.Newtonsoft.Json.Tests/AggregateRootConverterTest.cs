using System;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Savvyio.Assets.Domain;
using Savvyio.Domain;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    public class AggregateRootConverterTest : Test
    {
        public AggregateRootConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AggregateRootConverter_ShouldDeserializeUsingMatchingConstructor()
        {
            var id = Guid.NewGuid();
            var settings = CreateSettings();
            var json = JsonConvert.SerializeObject(new MatchingAggregate(id, "Jane Doe"), settings);

            var sut = JsonConvert.DeserializeObject<MatchingAggregate>(json, settings);

            Assert.NotNull(sut);
            Assert.Equal(id, sut.Id);
            Assert.Equal("Jane Doe", sut.Name);
        }

        [Fact]
        public void AggregateRootConverter_ShouldDeserializeUsingDefaultConstructorFallback()
        {
            var id = Guid.NewGuid();
            var providerId = new PlatformProviderId(Guid.NewGuid());
            var settings = CreateSettings();
            var json = JsonConvert.SerializeObject(new FallbackAggregate(id, "Jane Doe")
            {
                PlatformProviderId = providerId
            }, settings);

            var sut = JsonConvert.DeserializeObject<FallbackAggregate>(json, settings);

            Assert.NotNull(sut);
            Assert.Equal(id, sut.Id);
            Assert.Equal("Jane Doe", sut.Name);
            Assert.Equal(providerId, sut.PlatformProviderId);
        }

        [Fact]
        public void AggregateRootConverter_ShouldThrowWhenNoSuitableConstructorExists()
        {
            var settings = CreateSettings();
            var json = JsonConvert.SerializeObject(new UnsupportedAggregate("Jane Doe"), settings);

            var ex = Assert.Throws<InvalidOperationException>(() => JsonConvert.DeserializeObject<UnsupportedAggregate>(json, settings));

            Assert.StartsWith("Unable to deserialize", ex.Message);
        }

        private static JsonSerializerSettings CreateSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new AggregateRootConverter<Guid>());
            settings.Converters.Add(new SingleValueObjectConverter());
            return settings;
        }

        private sealed class MatchingAggregate : AggregateRoot<Guid>
        {
            public MatchingAggregate(Guid id, string name) : base(id)
            {
                Name = name;
            }

            public string Name { get; }
        }

        private sealed class FallbackAggregate : AggregateRoot<Guid>
        {
            public FallbackAggregate()
            {
            }

            public FallbackAggregate(Guid id, string name) : base(id)
            {
                Name = name;
            }

            public string Name { get; set; } = string.Empty;

            public PlatformProviderId PlatformProviderId { get; set; }
        }

        private sealed class UnsupportedAggregate : AggregateRoot<Guid>
        {
            public UnsupportedAggregate(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }
    }
}
