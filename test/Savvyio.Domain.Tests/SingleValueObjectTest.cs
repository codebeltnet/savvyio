using System;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain
{
    public class SingleValueObjectTest : Test
    {
        public SingleValueObjectTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void SingleValueObject_PlatformProviderId_ShouldBeEqual()
        {
            var id = Guid.NewGuid();
            var sut1 = new PlatformProviderId(id);
            var sut2 = new PlatformProviderId(id);

            TestOutput.WriteLines(sut1.ToString(), sut2.ToString());

            Assert.Equal(sut1, sut2);
        }

        [Fact]
        public void SingleValueObject_PlatformProviderId_ShouldNotBeEqual()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            var sut1 = new PlatformProviderId(id1);
            var sut2 = new PlatformProviderId(id2);

            TestOutput.WriteLines(sut1.ToString(), sut2.ToString());

            Assert.NotEqual(sut1, sut2);
        }

        [Fact]
        public void SingleValueObject_EmailAddress_ShouldConvertBiderictional()
        {
            var email = "root@gimlichael.dev";
            var sut = new EmailAddress(email);

            Assert.Equal(email, sut);
            Assert.Equal((EmailAddress)email, sut);
        }
    }
}
