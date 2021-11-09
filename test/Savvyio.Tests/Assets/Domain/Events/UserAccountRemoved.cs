using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class UserAccountRemoved : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public UserAccountRemoved()
        {
        }

        public UserAccountRemoved(UserAccount account)
        {
            Id = account.Id;
        }

        public long Id { get; private set; }
    }
}
