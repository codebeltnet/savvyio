using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public record UserAccountFailedLogonAttemptCreated : DomainEvent
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
