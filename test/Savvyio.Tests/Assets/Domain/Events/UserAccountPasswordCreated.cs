using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class UserAccountPasswordCreated : DomainEvent
    {
        public UserAccountPasswordCreated()
        {
        }

        public UserAccountPasswordCreated(UserAccountPassword password)
        {
            AccountId = password.AccountId;
            Hash = password.Hash;
            Salt = password.Salt;
        }

        public long AccountId { get; private set; }

        public string Hash { get; private set; }

        public string Salt { get; private set; }
    }
}
