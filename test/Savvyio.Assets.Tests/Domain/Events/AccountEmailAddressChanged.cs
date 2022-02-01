using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountEmailAddressChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public AccountEmailAddressChanged(Account account)
        {
            EmailAddress = account.EmailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}
