using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class PlatformProviderNameChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        PlatformProviderNameChanged()
        {
            
        }

        public PlatformProviderNameChanged(PlatformProvider provider)
        {
            Id = provider.Id;
            Name = provider.Name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }
    }
}
