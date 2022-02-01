using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class AccountFullNameChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public AccountFullNameChanged(Account account)
        {
            FullName = account.FullName;
        }

        public string FullName { get; private set; }
    }
}
