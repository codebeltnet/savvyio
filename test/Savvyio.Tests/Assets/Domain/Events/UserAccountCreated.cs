using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class UserAccountCreated : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public UserAccountCreated()
        {
        }

        public UserAccountCreated(UserAccount account)
        {
            UserName = account.UserName;
        }

        public string UserName { get; private set; }
    }
}
