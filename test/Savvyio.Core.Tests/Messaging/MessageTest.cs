using System;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets;
using Xunit;

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
            var sut = new Message<Request>(id, source, nameof(RootDummyRequest), data);

            TestOutput.WriteLine(sut.Data.ToString());

            var convertedTime = sut.Time!;
            Assert.Equal(id, sut.Id);
            Assert.Equal(source.OriginalString, sut.Source);
            Assert.Equal(data, sut.Data);
            Assert.Equal(data.GetType().ToFullNameIncludingAssemblyName(), sut.Data.GetMemberType());
            Assert.Equal(nameof(RootDummyRequest), sut.Type);
            Assert.Equal(DateTimeKind.Utc, convertedTime.Value.Kind);
            Assert.InRange(convertedTime.Value, utcNow.Subtract(TimeSpan.FromSeconds(2)), utcNow.Add(TimeSpan.FromSeconds(2)));
        }

        [Fact]
        public void Ctor_WithBothRequiredAndOptionalArguments()
        {
            var id = Generate.RandomString(16);
            var source = "urn:source".ToUri();
            var dataUuid = Guid.NewGuid();
            var dataNumber = Generate.RandomNumber();
            var dataType = typeof(DummyRequest);
            var type = "com.example.someevent";
            var data = new RootDummyRequest(dataUuid, dataNumber).SetMemberType(dataType);
            var utcNow = DateTime.UtcNow;
            var sut1 = new Message<Request>(id, source, type, data, utcNow);

            TestOutput.WriteLine(sut1.Data.ToString());

            var convertedTime = sut1.Time!;
            Assert.Equal(id, sut1.Id);
            Assert.Equal(source.OriginalString, sut1.Source);
            Assert.Equal(data, sut1.Data);
            Assert.Equal(type, sut1.Type);
            Assert.Equal(dataType.ToFullNameIncludingAssemblyName(), sut1.Data.GetMemberType());
            Assert.Equal(DateTimeKind.Utc, convertedTime.Value.Kind);
            Assert.Equal(utcNow, convertedTime);
        }
    }
}
