using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountInitiated : DomainEvent
    {
        public AccountInitiated()
        {
        }

        public AccountInitiated(Account account)
        {
            PlatformProviderId = account.PlatformProviderId;
            FullName = account.FullName;
            EmailAddress = account.EmailAddress;
        }

        public Guid PlatformProviderId { get; private set; }

        public string FullName { get; private set; }

        public string EmailAddress { get; private set; }
    }
}
