using System;
using Cuemon;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Domain
{
    public class DomainEventTest : Test
    {
        public DomainEventTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DomainEvent_AccountInitiated_ShouldHaveDefaultMetadata()
        {
            var fullname = "Michael Mortensen";
            var email = "root@gimlichael.dev";

            var account = new Account(Guid.NewGuid(), fullname, email);
            var sut = new AccountInitiated(account);
            var ts = DateTime.UtcNow;

            Assert.Equal(account.EmailAddress, sut.EmailAddress);
            Assert.Equal(account.FullName, sut.FullName);
            Assert.Equal(account.PlatformProviderId, sut.PlatformProviderId);
            Assert.Empty(account.Metadata);
            Assert.Collection(sut.Metadata.Keys,
                s => Assert.Equal(s, MetadataDictionary.MemberType),
                s => Assert.Equal(s, MetadataDictionary.EventId),
                s => Assert.Equal(s, MetadataDictionary.Timestamp));
                Assert.True(sut.Metadata.Count == 3, "sut.Metadata.Count == 3");
            Assert.InRange(DateTime.UtcNow, sut.GetTimestamp(), ts.AddSeconds(1));
        }

        [Fact]
        public void DomainEvent_AccountInitiated_ShouldHaveAdditionalMetadata_CopiedFromAggregate()
        {
            var id = Guid.NewGuid();
            var fullname = "Michael Mortensen";
            var email = "root@gimlichael.dev";

            var account = new Account(id, fullname, email)
                .SaveMetadata("Key1", "Key1Value")
                .SaveMetadata("Key2", "Key2Value")
                .SaveMetadata("correlationid", "makingSureCorrelationIdIsWrittenPascalCase");

            var sut = new AccountInitiated(account).MergeMetadata(account);
            var ts = DateTime.UtcNow;

            Assert.Equal(account.EmailAddress, sut.EmailAddress);
            Assert.Equal(account.FullName, sut.FullName);
            Assert.Equal(account.PlatformProviderId, sut.PlatformProviderId);
            Assert.Collection(sut.Metadata.Keys, 
                s => Assert.Equal(s, MetadataDictionary.MemberType),
                s => Assert.Equal(s, MetadataDictionary.EventId),
                s => Assert.Equal(s, MetadataDictionary.Timestamp),
                s => Assert.Equal(s, "Key1"),
                s => Assert.Equal(s, "Key2"),
                s => Assert.Equal(s, MetadataDictionary.CorrelationId));;
            Assert.True(sut.Metadata.Count == 6, "sut.Metadata.Count == 6");
            Assert.InRange(DateTime.UtcNow, sut.GetTimestamp(), ts.AddSeconds(1));
        }
    }
}
