using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountEmailAddressChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        AccountEmailAddressChanged()
        {
            
        }

        public AccountEmailAddressChanged(Account account)
        {
            Id = account.Id;
            EmailAddress = account.EmailAddress;
        }

        public long Id { get; private set; }

        public string EmailAddress { get; private set; }
    }
}
