using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountEmailAddressChanged : DomainEvent
    {
        public AccountEmailAddressChanged(Account account)
        {
            EmailAddress = account.EmailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}
