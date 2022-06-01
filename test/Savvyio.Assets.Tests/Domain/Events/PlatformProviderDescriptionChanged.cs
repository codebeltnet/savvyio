using System;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain.Events
{
    public class PlatformProviderDescriptionChanged : DomainEvent
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
