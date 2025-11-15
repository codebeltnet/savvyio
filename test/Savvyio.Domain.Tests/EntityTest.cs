using System;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Xunit;

namespace Savvyio.Domain
{
    public class EntityTest : Test
    {
        public EntityTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Entity_Account_ShouldBeTransient()
        {
            var sut = new Account(Guid.NewGuid(), "Michael Mortensen", "root@gimlichael.dev");

            Assert.Equal(0, sut.Id);
            Assert.True(sut.IsTransient);
            Assert.IsType<long>(sut.Id);
        }

        [Fact]
        public void Entity_Account_ShouldBeNonTransient()
        {
            var sut = new Account(100);

            Assert.Equal(100, sut.Id);
            Assert.False(sut.IsTransient);
            Assert.IsType<long>(sut.Id);
        }
    }
}
