using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class EmailAddress : SingleValueObject<string>
    {
        public static implicit operator EmailAddress(string value)
        {
            return new EmailAddress(value);
        }

        public EmailAddress(string value) : base(value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            Validator.ThrowIfNotEmailAddress(value, nameof(value));
        }
    }
}
