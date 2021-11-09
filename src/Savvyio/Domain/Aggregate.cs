using System.Collections.Generic;

namespace Savvyio.Domain
{
    public abstract class Aggregate<TKey, TEvent> : Entity<TKey>, IAggregateNotification<TEvent> where TEvent : IDomainEvent
    {
        private readonly List<TEvent> _events = new();

        protected Aggregate()
        {
        }

        protected Aggregate(TKey id)
        {
            Id = id;
        }

        public IReadOnlyList<TEvent> Events => _events;

        protected void AddEvent(TEvent @event)
        {
            _events.Add(@event);
        }

        public void RemoveAllEvents()
        {
            _events.Clear();
        }
    }
}
