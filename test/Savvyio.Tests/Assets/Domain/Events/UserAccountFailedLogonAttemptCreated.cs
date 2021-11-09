using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class UserAccountFailedLogonAttemptCreated : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public UserAccountFailedLogonAttemptCreated()
        {
        }

        public UserAccountFailedLogonAttemptCreated(UserAccountFailedLogonAttempt failedLogonAttempt)
        {
            AccountId = failedLogonAttempt.AccountId;
            UserHostAddress = failedLogonAttempt.UserHostAddress;
        }

        public long AccountId { get; private set; }

        public string UserHostAddress { get; private set; }
    }
}
