using System;
using Cuemon.Extensions.Reflection;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
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
        public void AggregateRoot_Account_ShouldHaveMetadata_FromCommand()
        {
            var id = Guid.NewGuid();
            var idString = id.ToString("N");
            var fullname = "Michael Mortensen";
            var email = "root@gimlichael.dev";
            var fakeMemberType = typeof(UpdateAccount);

            var command = new CreateAccount(id, fullname, email)
                .SetCausationId(idString)
                .SetCorrelationId(idString)
                .SetMemberType(fakeMemberType);
            
            var sut = new Account(command.PlatformProviderId, command.FullName, command.EmailAddress).MergeMetadata(command);
            
            Assert.Collection(sut.Metadata, 
                pair =>
                {
                    Assert.Equal(pair.Key, MetadataDictionary.MemberType);
                    Assert.Equal(pair.Value, fakeMemberType.ToFullNameIncludingAssemblyName());
                },
                pair =>
                {
                    Assert.Equal(pair.Key, MetadataDictionary.CorrelationId);
                    Assert.Equal(pair.Value, idString);
                },
                pair =>
                {
                    Assert.Equal(pair.Key, MetadataDictionary.CausationId);
                    Assert.Equal(pair.Value, idString);
                });

            Assert.True(sut.Metadata.Count == 3, "sut.Metadata.Count == 3");
        }

        [Fact]
        public void AggregateRoot_Account_ShouldHaveDefaultMetadata()
        {
            var fullname = "Michael Mortensen";
            var email = "root@gimlichael.dev";

            var sut = new Account(Guid.NewGuid(), fullname, email);

            Assert.True(sut.Metadata.Count == 0, "sut.Metadata.Count == 0");
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
