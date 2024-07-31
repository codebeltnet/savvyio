using System;
using Savvyio.Assets.Domain;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record CreateAccount : Command
    {
        public CreateAccount(PlatformProviderId platformProviderId, FullName fullName, EmailAddress emailAddress)
        {
            PlatformProviderId = platformProviderId;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public Guid PlatformProviderId { get; }

        public string FullName { get; }

        public string EmailAddress { get; }
    }
}
