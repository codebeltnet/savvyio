using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class UserAccountInitiated : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public UserAccountInitiated()
        {
        }

        public UserAccountInitiated(UserAccount account)
        {
            UserName = account.UserName;
        }

        public string UserName { get; private set; }
    }
}
