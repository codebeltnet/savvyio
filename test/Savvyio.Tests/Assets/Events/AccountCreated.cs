using Savvyio.Events;

namespace Savvyio.Assets.Events
{
    public class AccountCreated : IntegrationEvent
    {
        AccountCreated()
        {
        }

        public AccountCreated(long id, string fullName, string emailAddress)
        {
            Id = id;
            FullName = fullName;
            EmailAddress = emailAddress;
        }

        public long Id { get; private set; }

        public string FullName { get; private set; }

        public string EmailAddress { get; private set; }
    }
}
