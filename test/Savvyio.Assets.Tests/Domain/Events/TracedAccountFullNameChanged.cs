using Savvyio.Domain.EventSourcing;

namespace Savvyio.Assets.Domain.Events
{
    public class TracedAccountFullNameChanged : TracedDomainEvent // if naming conflict occur, consider this naming convention (to distinguish between domain- and integration events): <Entity>Instance<Action>
    {
        public TracedAccountFullNameChanged(string fullName)
        {
            FullName = fullName;
        }

        public string FullName { get; private set; }
    }
}
