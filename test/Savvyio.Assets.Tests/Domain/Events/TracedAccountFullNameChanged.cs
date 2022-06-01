using Savvyio.Domain.EventSourcing;

namespace Savvyio.Assets.Domain.Events
{
    public class TracedAccountFullNameChanged : TracedDomainEvent
    {
        public TracedAccountFullNameChanged(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; private set; }
    }
}
