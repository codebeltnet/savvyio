using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public class CreatePlatformProvider : ICommand
    {
        public CreatePlatformProvider(string name, string thirdLevelDomainName, string description = null)
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
