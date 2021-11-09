using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class PlatformProviderThirdLevelDomainNameChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
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
