using System;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain
{
    public class AggregateRootTest : Test
    {
        public AggregateRootTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AggregateRoot_PlatformProvider_ShouldRaise_PlatformProviderInitiated_Event()
        {
            var name = "AmaZure";
            var thirdLvlDomainName = "amz";
            var description = "The next big thing in cloud providers!";

            var sut = new PlatformProvider(name, thirdLvlDomainName, description);

            Assert.Equal(sut.Description, description);
            Assert.Equal(sut.Name, name);
            Assert.Equal(sut.ThirdLevelDomainName, thirdLvlDomainName);
            Assert.Equal(sut.Policy, new PlatformProviderAccountPolicy());
            Assert.True(sut.Events.Count == 1, "sut.Events.Count == 1");
            Assert.Collection(sut.Events, e => Assert.IsType<PlatformProviderInitiated>(e));
        }

        [Fact]
        public void AggregateRoot_PlatformProvider_ShouldRaise_FourDifferent_Events()
        {
            var name = "AmaZure";
            var thirdLvlDomainName = "amz";
            var description = "The next big thing in cloud providers!";
            var newAccountPolicy = new PlatformProviderAccountPolicy(5, 0, TimeSpan.Zero, TimeSpan.Zero, 0, TimeSpan.Zero, TimeSpan.Zero, false);
            var newDescription = "The next big thing in platform providers in the cloud!";
            var newName = "AwaZure";
            var newThirdLvlDomainName = "awz";

            var sut = new PlatformProvider(name, thirdLvlDomainName, description);
            sut.RemoveAllEvents();

            Assert.Equal(sut.Description, description);
            Assert.Equal(sut.Name, name);
            Assert.Equal(sut.ThirdLevelDomainName, thirdLvlDomainName);
            Assert.Equal(sut.Policy, new PlatformProviderAccountPolicy());
            Assert.True(sut.Events.Count == 0, "sut.Events.Count == 0");
            
            sut.ChangeAccountPolicy(newAccountPolicy);
            sut.ChangeDescription(newDescription);
            sut.ChangeName(newName);
            sut.ChangeThirdLevelDomainName(newThirdLvlDomainName);

            Assert.Equal(sut.Description, newDescription);
            Assert.Equal(sut.Name, newName);
            Assert.Equal(sut.ThirdLevelDomainName, newThirdLvlDomainName);
            Assert.Equal(sut.Policy, newAccountPolicy);
            Assert.True(sut.Events.Count == 4, "sut.Events.Count == 4");

            Assert.Collection(sut.Events,
                e => Assert.IsType<PlatformProviderAccountPolicyChanged>(e),
                e => Assert.IsType<PlatformProviderDescriptionChanged>(e),
                e => Assert.IsType<PlatformProviderNameChanged>(e),
                e => Assert.IsType<PlatformProviderThirdLevelDomainNameChanged>(e));
        }
    }
}
