using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record EmailAddress : SingleValueObject<string>
    {
        public static implicit operator EmailAddress(string value)
        {
            return new EmailAddress(value);
        }

        public EmailAddress(string value) : base(value)
        {
            Validator.ThrowIfNullOrWhitespace(value);
            Validator.ThrowIfNotEmailAddress(value);
        }
    }
}
