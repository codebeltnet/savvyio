using System;
using Codebelt.Extensions.Xunit;
using Newtonsoft.Json;
using Savvyio.Domain;
using Xunit;

namespace Savvyio.Extensions.Newtonsoft.Json.Converters
{
    public class ValueObjectConverterTest : Test
    {
        public ValueObjectConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WriteJson_ShouldSerializeValueObjectWithSimpleProperties()
        {
            var sut = new SimplePoint(3, 7);
            var settings = CreateSettings();

            var json = JsonConvert.SerializeObject(sut, settings);

            TestOutput.WriteLine(json);

            Assert.Contains("X", json);
            Assert.Contains("Y", json);
            Assert.Contains("3", json);
            Assert.Contains("7", json);
        }

        [Fact]
        public void WriteJson_ShouldSerializeSingleValueObjectProperty()
        {
            var sut = new LabeledPoint("origin", new PointLabel("O"));
            var settings = CreateSettings();

            var json = JsonConvert.SerializeObject(sut, settings);

            TestOutput.WriteLine(json);

            Assert.Contains("Name", json);
            Assert.Contains("Label", json);
            Assert.Contains("origin", json);
            Assert.Contains("O", json);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeSingleValueObjectScalarWhenNested()
        {
            var original = new LabeledPoint("center", new PointLabel("C"));
            var settings = CreateSettings();

            var json = JsonConvert.SerializeObject(original, settings);
            TestOutput.WriteLine(json);

            var result = JsonConvert.DeserializeObject<LabeledPoint>(json, settings);

            Assert.NotNull(result);
            Assert.Equal("center", result.Name);
            Assert.NotNull(result.Label);
            Assert.Equal("C", result.Label.Value);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeComplexValueObjectWithMatchingCtor()
        {
            var original = new SimplePoint(1, 2);
            var settings = CreateSettings();

            var json = JsonConvert.SerializeObject(original, settings);
            TestOutput.WriteLine(json);

            var result = JsonConvert.DeserializeObject<SimplePoint>(json, settings);

            Assert.NotNull(result);
            Assert.Equal(1, result.X);
            Assert.Equal(2, result.Y);
        }

        [Fact]
        public void ReadJson_ShouldDeserializeValueObjectWithDefaultCtor()
        {
            var original = new MutablePoint { X = 5, Y = 6 };
            var settings = CreateSettings();

            var json = JsonConvert.SerializeObject(original, settings);
            TestOutput.WriteLine(json);

            var result = JsonConvert.DeserializeObject<MutablePoint>(json, settings);

            Assert.NotNull(result);
            Assert.Equal(5, result.X);
            Assert.Equal(6, result.Y);
        }

        [Fact]
        public void ReadJson_ShouldThrowInvalidOperationException_WhenNoSuitableCtorExists()
        {
            var settings = CreateSettings();
            var json = JsonConvert.SerializeObject(new NoSuitableCtorPoint("a", "b"), settings);
            TestOutput.WriteLine(json);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                JsonConvert.DeserializeObject<NoSuitableCtorPoint>(json, settings));

            Assert.Contains("Unable to deserialize", ex.Message);
        }

        private static JsonSerializerSettings CreateSettings()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ValueObjectConverter());
            return settings;
        }

        private record SimplePoint : ValueObject
        {
            public SimplePoint(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }

        private sealed record PointLabel : SingleValueObject<string>
        {
            public PointLabel(string value) : base(value) { }
        }

        private record LabeledPoint : ValueObject
        {
            public LabeledPoint(string name, PointLabel label)
            {
                Name = name;
                Label = label;
            }

            public string Name { get; }
            public PointLabel Label { get; }
        }

        private record MutablePoint : ValueObject
        {
            public MutablePoint() { }

            public int X { get; set; }
            public int Y { get; set; }
        }

        private record NoSuitableCtorPoint : ValueObject
        {
            public NoSuitableCtorPoint(string a, string b, string c = null)
            {
                A = a;
                B = b;
            }

            public string A { get; }
            public string B { get; }
        }
    }
}
