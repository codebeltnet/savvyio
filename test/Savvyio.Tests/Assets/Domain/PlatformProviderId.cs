using System;
using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public sealed class PlatformProviderId : SingleValueObject<Guid>
    {
        public static implicit operator PlatformProviderId(Guid value)
        {
            return new PlatformProviderId(value);
        }

        public PlatformProviderId(Guid id) : base(id)
        {
            Validator.ThrowIfEqual(id, Guid.Empty, nameof(id));
        }
    }
}
