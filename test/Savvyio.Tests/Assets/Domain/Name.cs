using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class Name : SingleValueObject<string>
    {
        public static implicit operator Name(string value)
        {
            return new Name(value);
        }

        public Name(string value) : base(value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
        }
    }
}
