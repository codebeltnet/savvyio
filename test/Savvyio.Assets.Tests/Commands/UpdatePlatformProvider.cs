using System;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record UpdatePlatformProvider : Command
    {
        public UpdatePlatformProvider(Guid id, string name, string thirdLevelDomainName, string description = null)
        {
            Id = id;
            Name = name;
            ThirdLevelDomainName = thirdLevelDomainName;
            Description = description;
        }

        public Guid Id { get; }

        public string Name { get; }
        
        public string ThirdLevelDomainName { get; }

        public string Description { get; }
    }
}
