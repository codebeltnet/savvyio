using System;

namespace Savvyio.Assets.Queries
{
    public class PlatformProviderProjection
    {
        public PlatformProviderProjection()
        {
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ThirdLevelDomainName { get; set; }

        public string Description { get; set; }

        public int MinimumPasswordLength { get; set; }

        public int EnforcePasswordHistory { get; set; }

        public long MinimumPasswordAge { get; set; }

        public long MaximumPasswordAge { get; set; }

        public int AccountLockoutThreshold { get; set; }

        public long AccountLockoutDuration { get; set; }

        public long AccountLockoutCounterReset { get; set; }

        public bool PasswordComplexityRequirement { get; set; }
    }
}
