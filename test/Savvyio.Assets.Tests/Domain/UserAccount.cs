using System;
using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class UserAccount : Entity<long>
    {
        public UserAccount(AccountId accountId, string userName) : base(accountId)
        {
            Validator.ThrowIfNullOrWhitespace(userName);
            UserName = userName;
        }

        public UserAccount(AccountId id) : base(id)
        {
            
        }

        public string UserName { get; private set; }

        public int FailedLogonAttemptCount { get; private set; }

        public int SuccessfulLogonCount { get; private set; }

        public DateTimeOffset LastLogon { get; private set; }

        public DateTimeOffset AccountLockout { get; private set; }

        public DateTimeOffset AccountLockoutReset { get; private set; }
    }
}
