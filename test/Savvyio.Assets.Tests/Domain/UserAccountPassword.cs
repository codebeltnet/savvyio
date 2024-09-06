using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class UserAccountPassword : Entity<long>
    {
        public UserAccountPassword(AccountId accountId, Credentials password)
        {
            Validator.ThrowIfNull(accountId);
            Validator.ThrowIfNull(password);

            AccountId = accountId;
            Hash = password.Hash;
            Salt = password.Salt;
        }

        public long AccountId { get; private set; }

        public string Hash { get; private set; }

        public string Salt { get; private set; }
    }
}
