using System;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public class CreateAccount : Command
    {
        public CreateAccount(Guid platformProviderId, string fullName, string emailAddress)
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
