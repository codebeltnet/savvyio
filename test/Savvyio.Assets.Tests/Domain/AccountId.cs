using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public sealed record AccountId : SingleValueObject<long>
    {
        public static implicit operator AccountId(long value)
        {
            return new AccountId(value);
        }

        public AccountId(long id) : base(id)
        {
            Validator.ThrowIfLowerThanOrEqual(id, 0, nameof(id));
        }
    }
}
