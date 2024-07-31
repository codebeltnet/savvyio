using Savvyio.Assets.Domain;
using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record UpdateAccount : Command
    {
        public UpdateAccount(AccountId id, FullName fullName, EmailAddress emailAddress)
        {
            Id = id;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public long Id { get; }

        public string FullName { get; }

        public string EmailAddress { get; }
    }
}
