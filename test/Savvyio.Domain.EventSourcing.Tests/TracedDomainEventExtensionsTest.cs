using System;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Domain.Events;
using Xunit;

namespace Savvyio.Domain.EventSourcing
{
    public class TracedDomainEventExtensionsTest : Test
    {
        public TracedDomainEventExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TracedDomainEventExtensions_ShouldSetAndGetAggregateVersion()
        {
            var domainEvent = new TracedAccountEmailAddressChanged("test@unit.test");

            var sut = domainEvent.SetAggregateVersion(42);

            Assert.Same(domainEvent, sut);
            Assert.Equal(42, sut.GetAggregateVersion());
        }

        [Fact]
        public void TracedDomainEventExtensions_ShouldReturnMemberTypeMetadata()
        {
            var sut = new TracedAccountInitiated(Guid.NewGuid(), Guid.NewGuid(), "Jane Doe", "jd@office.com");

            var memberType = sut.GetMemberType();

            Assert.StartsWith(typeof(TracedAccountInitiated).FullName, memberType);
            Assert.Contains("Savvyio.Assets.Tests", memberType);
        }
    }
}
