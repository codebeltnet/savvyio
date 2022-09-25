using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public record PlatformProviderThirdLevelDomainNameChanged : DomainEvent
    {
        PlatformProviderThirdLevelDomainNameChanged()
        {
        }

        public PlatformProviderThirdLevelDomainNameChanged(PlatformProvider provider)
        {
            Id = provider.Id;
            ThirdLevelDomainName = provider.ThirdLevelDomainName;
        }

        public Guid Id { get; private set; }

        public string ThirdLevelDomainName { get; private set; }
    }
}
