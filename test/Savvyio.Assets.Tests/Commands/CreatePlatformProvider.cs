using Savvyio.Assets.Domain;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record CreatePlatformProvider : Command
    {
        public CreatePlatformProvider(Name name, ThirdLevelDomainName thirdLevelDomainName, Description description = null)
        {
            Name = name;
            ThirdLevelDomainName = thirdLevelDomainName;
            Description = description;
        }

        public string Name { get; }
        
        public string ThirdLevelDomainName { get; }

        public string Description { get; }
    }
}
