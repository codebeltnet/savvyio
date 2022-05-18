using Savvyio.Domain.EventSourcing;

namespace Savvyio.Assets.Domain.Events
{
    public class TracedAccountEmailAddressChanged : TracedDomainEvent
    {
        public TracedAccountEmailAddressChanged(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}
