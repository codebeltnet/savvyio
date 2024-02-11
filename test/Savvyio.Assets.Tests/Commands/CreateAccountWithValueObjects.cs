using Savvyio.Assets.Domain;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record CreateAccountWithValueObjects : Command
    {
        public CreateAccountWithValueObjects(PlatformProviderId platformProviderId, FullName fullName, EmailAddress emailAddress)
        {
            PlatformProviderId = platformProviderId;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public PlatformProviderId PlatformProviderId { get; }

        public FullName FullName { get; }

        public EmailAddress EmailAddress { get; }
    }
}
