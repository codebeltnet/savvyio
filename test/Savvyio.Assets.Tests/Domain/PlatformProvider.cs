using System;
using Cuemon;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class PlatformProvider : AggregateRoot<Guid>
    {
        PlatformProvider() // efcore
        {
        }

        public PlatformProvider(Name name, ThirdLevelDomainName thirdLevelDomainName, Description description = null)
        {
            Validator.ThrowIfNull(name);
            Validator.ThrowIfNull(thirdLevelDomainName);
            Id = Guid.NewGuid();
            Name = name;
            ThirdLevelDomainName = thirdLevelDomainName;
            Description = description ?? new Description();
            Policy = new PlatformProviderAccountPolicy();
            AddEvent(new PlatformProviderInitiated(this));
        }

        public PlatformProvider(PlatformProviderId id) : base(id)
        {
        }

        public string Name { get; private set; }

        public string ThirdLevelDomainName { get; private set; }

        public string Description { get; private set; }

        public PlatformProviderAccountPolicy Policy { get; private set; }

        public void ChangeName(Name name)
        {
            Validator.ThrowIfNullOrWhitespace(name);
            if (name.Equals((Name)Name)) { return; }
            Name = name;
            AddEvent(new PlatformProviderNameChanged(this));
        }

        public void ChangeThirdLevelDomainName(ThirdLevelDomainName thirdLevelDomainName)
        {
            Validator.ThrowIfNull(thirdLevelDomainName);
            if (thirdLevelDomainName.Equals((ThirdLevelDomainName)ThirdLevelDomainName)) { return; }
            ThirdLevelDomainName = thirdLevelDomainName;
            AddEvent(new PlatformProviderThirdLevelDomainNameChanged(this));
        }

        public void ChangeDescription(Description description)
        {
            Validator.ThrowIfNull(description);
            if (Description != null && description.Equals((Description)Description)) { return; }
            Description = description;
            AddEvent(new PlatformProviderDescriptionChanged(this));
        }

        public void ChangeAccountPolicy(PlatformProviderAccountPolicy policy)
        {
            Validator.ThrowIfNull(policy);
            if (policy.Equals(Policy)) { return; }
            Policy = policy;
            AddEvent(new PlatformProviderAccountPolicyChanged(this));
        }
    }
}
