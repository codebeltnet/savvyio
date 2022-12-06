using System;
using System.Globalization;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Messaging
{
    public class MessageTest : Test
    {
        public MessageTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Ctor_WithRequiredArguments_RemainingArgumentsShouldHaveDefaultValues()
        {
            var id = Generate.RandomString(16);
            var source = "urn:source".ToUri();
            var dataUuid = Guid.NewGuid();
            var dataNumber = Generate.RandomNumber();
            var data = new RootDummyRequest(dataUuid, dataNumber);
            var utcNow = DateTime.UtcNow;
            var sut = new Message<Request>(id, source, data);

            TestOutput.WriteLine(sut.Data.ToString());

            var convertedTime = DateTime.Parse(sut.Time, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            Assert.Equal(id, sut.Id);
            Assert.Equal(source.OriginalString, sut.Source);
            Assert.Equal(data, sut.Data);
            Assert.Equal(data.GetType().ToFullNameIncludingAssemblyName(), sut.Type);
            Assert.Equal(DateTimeKind.Utc, convertedTime.Kind);
            Assert.InRange(convertedTime, utcNow.Subtract(TimeSpan.FromSeconds(2)), utcNow.Add(TimeSpan.FromSeconds(2)));
        }

        [Fact]
        public void Ctor_WithBothRequiredAndOptionalArguments()
        {
            var id = Generate.RandomString(16);
            var source = "urn:source".ToUri();
            var dataUuid = Guid.NewGuid();
            var dataNumber = Generate.RandomNumber();
            var dataType = typeof(DummyRequest);
            var data = new RootDummyRequest(dataUuid, dataNumber);
            var utcNow = DateTime.UtcNow;
            var sut1 = new Message<Request>(id, source, data, dataType, utcNow);

            TestOutput.WriteLine(sut1.Data.ToString());

            var convertedTime = DateTime.Parse(sut1.Time, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            Assert.Equal(id, sut1.Id);
            Assert.Equal(source.OriginalString, sut1.Source);
            Assert.Equal(data, sut1.Data);
            Assert.Equal(dataType.ToFullNameIncludingAssemblyName(), sut1.Type);
            Assert.Equal(DateTimeKind.Utc, convertedTime.Kind);
            Assert.Equal(utcNow, convertedTime);
        }
    }
}
