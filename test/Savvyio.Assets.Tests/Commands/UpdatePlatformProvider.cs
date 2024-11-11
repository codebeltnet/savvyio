using System;
using Savvyio.Assets.Domain;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record UpdatePlatformProvider : Command
    {
        public UpdatePlatformProvider(PlatformProviderId id, Name name, ThirdLevelDomainName thirdLevelDomainName, Description description = null)
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
