using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class PlatformProviderDescriptionChanged : DomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        PlatformProviderDescriptionChanged()
        {
        }

        public PlatformProviderDescriptionChanged(PlatformProvider provider)
        {
            Id = provider.Id;
            Description = provider.Description;
        }

        public Guid Id { get; private set; }

        public string Description { get; private set; }
    }
}
