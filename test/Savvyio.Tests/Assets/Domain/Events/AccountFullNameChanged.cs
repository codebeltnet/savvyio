using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountFullNameChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        AccountFullNameChanged()
        {
        }

        public AccountFullNameChanged(Account account)
        {
            Id = account.Id;
            FullName = account.FullName;
        }

        public long Id { get; private set; }

        public string FullName { get; private set; }
    }
}
