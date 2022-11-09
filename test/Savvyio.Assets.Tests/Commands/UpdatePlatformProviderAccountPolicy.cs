using System;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record UpdatePlatformProviderAccountPolicy : Command
    {
        public UpdatePlatformProviderAccountPolicy(Guid id, int minimumPasswordLength, int enforcePasswordHistory, TimeSpan minimumPasswordAge, TimeSpan maximumPasswordAge, int accountLockoutThreshold, TimeSpan accountLockoutDuration, TimeSpan accountLockoutCounterReset, bool passwordComplexityRequirement)
        {
            Id = id;
            MinimumPasswordLength = minimumPasswordLength;
            EnforcePasswordHistory = enforcePasswordHistory;
            MinimumPasswordAge = minimumPasswordAge;
            MaximumPasswordAge = maximumPasswordAge;
            AccountLockoutThreshold = accountLockoutThreshold;
            AccountLockoutDuration = accountLockoutDuration;
            AccountLockoutCounterReset = accountLockoutCounterReset;
            PasswordComplexityRequirement = passwordComplexityRequirement;
        }

        public Guid Id { get; }

        public int MinimumPasswordLength { get; }

        public int EnforcePasswordHistory { get; }

        public TimeSpan MinimumPasswordAge { get; }

        public TimeSpan MaximumPasswordAge { get; }

        public int AccountLockoutThreshold { get; }

        public TimeSpan AccountLockoutDuration { get; }

        public TimeSpan AccountLockoutCounterReset { get; }

        public bool PasswordComplexityRequirement { get; }
    }
}
