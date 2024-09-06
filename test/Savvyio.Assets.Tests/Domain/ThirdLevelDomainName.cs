using Cuemon;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public record ThirdLevelDomainName : SingleValueObject<string>
    {
        public static implicit operator ThirdLevelDomainName(string value)
        {
            return new ThirdLevelDomainName(value);
        }

        public ThirdLevelDomainName(string value) : base(value)
        {
            Validator.ThrowIfNullOrWhitespace(value);
            Validator.ThrowIfDifferent(Alphanumeric.Letters, value, nameof(value), $"One or more invalid character(s) specified. Allowed characters are: {Alphanumeric.Letters}");
        }
    }
}
