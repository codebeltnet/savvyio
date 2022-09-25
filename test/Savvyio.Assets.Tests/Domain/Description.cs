using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record Description : SingleValueObject<string>
    {
        public static implicit operator Description(string value)
        {
            return new Description(value);
        }

        public Description(string value = null) : base(value)
        {
            Validator.CheckParameter(value, () =>
            {
                if (value != null) { Validator.ThrowIfGreaterThan(value.Length, 1024, nameof(value)); }
            });
        }
    }
}
