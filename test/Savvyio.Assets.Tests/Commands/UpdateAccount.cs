using Savvyio.Commands;

namespace Savvyio.Assets.Commands
{
    public record UpdateAccount : Command
    {
        public UpdateAccount(long id, string fullName, string emailAddress)
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
