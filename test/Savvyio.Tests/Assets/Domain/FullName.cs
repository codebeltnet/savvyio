using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class FullName : SingleValueObject<string>
    {
        public static implicit operator FullName(string value)
        {
            return new FullName(value);
        }

        public FullName(string firstName, string lastName) : this($"{firstName?.Trim()} {lastName?.Trim()}")
        {
        }

        public FullName(string value) : base(value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
        }
    }
}
