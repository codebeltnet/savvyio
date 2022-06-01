using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class UserAccountInitiated : DomainEvent
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
