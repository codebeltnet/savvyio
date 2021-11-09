using Cuemon;
using Cuemon.Extensions;
using Savvyio.Domain;

namespace Savvyio.Assets.Domain
{
    public class ThirdLevelDomainName : SingleValueObject<string>
    {
        public static implicit operator ThirdLevelDomainName(string value)
        {
            return new ThirdLevelDomainName(value);
        }

        public ThirdLevelDomainName(string value) : base(value)
        {
            Validator.ThrowIfNullOrWhitespace(value, nameof(value));
            Validator.ThrowIf.HasDifference(Alphanumeric.Letters, value, nameof(value));
        }
    }
}
