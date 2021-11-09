using System.Collections.Generic;

namespace Savvyio.Domain
{
    public interface IAggregateNotification<out T> where T : IDomainEvent
    {
        IReadOnlyList<T> Events { get; }

        void RemoveAllEvents();
    }
}
