using Cuemon;
using Cuemon.Extensions;
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
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            Validator.ThrowIf.HasDifference(Alphanumeric.Letters, value, nameof(value), $"One or more invalid character(s) specified. Allowed characters are: {Alphanumeric.Letters}");
        }
    }
}
