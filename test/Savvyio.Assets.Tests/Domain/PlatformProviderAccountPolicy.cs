using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record PlatformProviderAccountPolicy : ValueObject
    {
        public PlatformProviderAccountPolicy() : this(8, 0, TimeSpan.Zero, TimeSpan.Zero, 10, TimeSpan.Zero, TimeSpan.Zero, true)
        {
        }

        public PlatformProviderAccountPolicy(int minimumPasswordLength, int enforcePasswordHistory, TimeSpan minimumPasswordAge, TimeSpan maximumPasswordAge, int accountLockoutThreshold, TimeSpan accountLockoutDuration, TimeSpan accountLockoutCounterReset, bool passwordComplexityRequirement)
        {
            MinimumPasswordLength = minimumPasswordLength;
            EnforcePasswordHistory = enforcePasswordHistory;
            MinimumPasswordAge = minimumPasswordAge.Ticks;
            MaximumPasswordAge = maximumPasswordAge.Ticks;
            AccountLockoutThreshold = accountLockoutThreshold;
            AccountLockoutDuration = accountLockoutDuration.Ticks;
            AccountLockoutCounterReset = accountLockoutCounterReset.Ticks;
            PasswordComplexityRequirement = passwordComplexityRequirement;
        }

        public int MinimumPasswordLength { get; private set; }

        public int EnforcePasswordHistory { get; private set; }

        public long MinimumPasswordAge { get; private set; }

        public long MaximumPasswordAge { get; private set; }

        public int AccountLockoutThreshold { get; private set; }

        public long AccountLockoutDuration { get; private set; }

        public long AccountLockoutCounterReset { get; private set; }

        public bool PasswordComplexityRequirement { get; private set; }
    }
}
