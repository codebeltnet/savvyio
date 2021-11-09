using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a way to cover the pattern of an Aggregate as specified in Domain Driven Design that is optimized for Event Sourcing. This is an abstract class.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="Aggregate{TKey,TEvent}" />
    /// <seealso cref="ITracedAggregateRoot{TKey}" />
    public abstract class TracedAggregateRoot<TKey> : Aggregate<TKey, ITracedDomainEvent>, ITracedAggregateRoot<TKey>
    {
        private readonly HandlerManager<ITracedDomainEvent> _handlers = new();

        protected TracedAggregateRoot()
        {
            Initialize();
        }

        protected TracedAggregateRoot(TKey id, IEnumerable<ITracedDomainEvent> events) : base(id)
        {
            Initialize();
            ReplayEvents(events);
        }

        private void Initialize()
        {
            RegisterTracedDomainEventHandlers(_handlers);
        }

        protected abstract void RegisterTracedDomainEventHandlers(IHandlerRegistry<ITracedDomainEvent> handler);

        public long Version { get; private set; }

        public void ReplayEvents(IEnumerable<ITracedDomainEvent> events)
        {
            if (events == null) { return; }
            foreach (var e in events)
            {
                Version = e.Version;
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(ITracedDomainEvent e, bool isNew = true)
        {
            if (e == null) { return; }
            if (isNew)
            {
                if (Version == 0) { RaiseEvent(e); }
                e.Version = Version + 1;
                Version = e.Version;
                AddEvent(e);
            }
            else
            {
                RaiseEvent(e);
            }
        }

        private void RaiseEvent(ITracedDomainEvent @event)
        {
            _handlers.TryInvoke(@event);
        }
    }
}
