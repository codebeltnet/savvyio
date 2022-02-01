using Savvyio.Domain.EventSourcing;

namespace Savvyio.Assets.Domain.Events
{
    public class TracedAccountEmailAddressChanged : TracedDomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public TracedAccountEmailAddressChanged(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}
