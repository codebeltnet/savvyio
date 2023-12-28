using System;
using Savvyio.Assets.Domain;
using Savvyio.EventDriven;

namespace Savvyio.Assets.EventDriven
{
    public record PlatformProviderUpdated : IntegrationEvent
    {
        PlatformProviderUpdated()
        {
        }

        public PlatformProviderUpdated(PlatformProvider provider)
        {
            Id = provider.Id;
            Name = provider.Name;
            ThirdLevelDomainName = provider.ThirdLevelDomainName;
            Description = provider.Description;
            Policy = provider.Policy;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string ThirdLevelDomainName { get; private set; }

        public string Description { get; private set; }

        public PlatformProviderAccountPolicy Policy { get; private set; }
    }
}
