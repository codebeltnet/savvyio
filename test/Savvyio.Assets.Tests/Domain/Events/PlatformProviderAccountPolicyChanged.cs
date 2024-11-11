using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public record PlatformProviderAccountPolicyChanged : DomainEvent
    {
        PlatformProviderAccountPolicyChanged()
        {

        }

        public PlatformProviderAccountPolicyChanged(PlatformProvider provider)
        {
            Id = provider.Id;
            Policy = provider.Policy;
        }

        public Guid Id { get; private set; }


        public PlatformProviderAccountPolicy Policy { get; private set; }
    }
}
