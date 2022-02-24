using System;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Savvyio.Domain.Assets;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain
{
    public class ValueObjectTest : Test
    {
        public ValueObjectTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ValueObject_PlatformProviderAccountPolicy_ShouldBeEqual()
        {
            var sut1 = new PlatformProviderAccountPolicy();
            var sut2 = new PlatformProviderAccountPolicy();

            TestOutput.WriteLines(sut1.ToString(), sut2.ToString());

            Assert.True(sut1 == sut2);
            Assert.Equal(sut1, sut2);
            Assert.Equal(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Fact]
        public void ValueObject_PlatformProviderAccountPolicy_ShouldNotBeEqual()
        {
            var sut1 = new PlatformProviderAccountPolicy();
            var sut2 = new PlatformProviderAccountPolicy(1, 24, TimeSpan.Zero, TimeSpan.FromDays(1), 5, TimeSpan.FromHours(1), TimeSpan.FromMinutes(10), false);

            TestOutput.WriteLines(sut1.ToString(), sut2.ToString());

            Assert.True(sut1 != sut2);
            Assert.NotEqual(sut1, sut2);
            Assert.NotEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }

        [Fact]
        public void ValueObject_Money_ShouldNotBeEqual()
        {
            var sut1 = new Money(Currency.MYR, 100);
            var sut2 = new Money(Currency.USD, 101);

            TestOutput.WriteLines(sut1.ToString(), sut2.ToString());

            Assert.True(sut1 != sut2);
            Assert.NotEqual(sut1, sut2);
            Assert.NotEqual(sut1.GetHashCode(), sut2.GetHashCode());
        }
    }
}
