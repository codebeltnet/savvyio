using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountInitiated : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
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
