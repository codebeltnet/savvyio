﻿using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class UserAccountFailedLogonAttempt : Entity<long>
    {
        public UserAccountFailedLogonAttempt(AccountId accountId, string userHostAddress)
        {
            Validator.ThrowIfNull(accountId, nameof(accountId));
            Validator.ThrowIfNullOrWhitespace(userHostAddress, nameof(userHostAddress));

            AccountId = accountId;
            UserHostAddress = userHostAddress;
        }

        public long AccountId { get; private set; }

        public string UserHostAddress { get; private set; }
    }
}
