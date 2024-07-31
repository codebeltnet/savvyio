using System;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Assets.Domain.Events
{
    public record TracedAccountInitiated : TracedDomainEvent
    {
        public TracedAccountInitiated(Guid id, Guid platformProviderId, string fullName, string emailAddress)
        {
            Id = id;
            PlatformProviderId = platformProviderId;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public Guid Id { get; }

        public Guid PlatformProviderId { get; }

        public string FullName { get; }

        public string EmailAddress { get; }
    }
}
