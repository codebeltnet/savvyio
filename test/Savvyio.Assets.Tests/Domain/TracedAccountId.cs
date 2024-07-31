using System;
using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record TracedAccountId : SingleValueObject<Guid>
    {
        public static implicit operator TracedAccountId(Guid value)
        {
            return new TracedAccountId(value);
        }

        public TracedAccountId(Guid id) : base(id)
        {
            Validator.ThrowIfTrue(() => id == Guid.Empty, nameof(id), "Value cannot be an empty UUID.");
        }
    }
}
