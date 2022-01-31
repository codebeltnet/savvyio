using System.Collections.Generic;
using Savvyio.Handlers;

namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Provides a way to cover the pattern of an Aggregate as specified in Domain Driven Design that is optimized for Event Sourcing. This is an abstract class.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="Aggregate{TKey,TEvent}" />
    /// <seealso cref="ITracedAggregateRoot{TKey}" />
    public abstract class TracedAggregateRoot<TKey> : Aggregate<TKey, ITracedDomainEvent>, ITracedAggregateRoot<TKey>
    {
        private IFireForgetActivator<ITracedDomainEvent> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAggregateRoot{TKey}"/> class.
        /// </summary>
        /// <param name="metadata">The optional metadata to merge with this instance.</param>
        protected TracedAggregateRoot(IMetadata metadata = null) : base(metadata)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TracedAggregateRoot{TKey}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="events">The events to rehydrate from.</param>
        protected TracedAggregateRoot(TKey id, IEnumerable<ITracedDomainEvent> events) : base(id)
        {
            Initialize();
            ReplayEvents(events);
        }

        private void Initialize()
        {
            _handlers = HandlerFactory.CreateFireForget<ITracedDomainEvent>(RegisterDelegates);
        }

        /// <summary>
        /// Registers the delegates responsible of handling types that implements the <see cref="ITracedDomainEvent"/> interface.
        /// </summary>
        /// <param name="handler">The registry that store the delegates of type <see cref="ITracedDomainEvent"/>.</param>
        protected abstract void RegisterDelegates(IFireForgetRegistry<ITracedDomainEvent> handler);

        /// <summary>
        /// Gets the version of the Aggregate.
        /// </summary>
        /// <value>The version of the Aggregate.</value>
        public long Version { get; private set; }

        private void ReplayEvents(IEnumerable<ITracedDomainEvent> events)
        {
            if (events == null) { return; }
            foreach (var e in events)
            {
                Version = e.GetAggregateVersion();
                ApplyChange(e, false);
            }
        }

        /// <summary>
        /// Adds an event to the Aggregate.
        /// </summary>
        /// <param name="e">The event to be added to the end of <see cref="P:Savvyio.Domain.Aggregate`2.Events" />.</param>
        protected sealed override void AddEvent(ITracedDomainEvent e)
        {
            ApplyChange(e);
        }

        private void ApplyChange(ITracedDomainEvent e, bool isNew = true)
        {
            if (e == null) { return; }
            if (isNew)
            {
                RaiseEvent(e);
                e.SetAggregateVersion(Version + 1);
                Version = e.GetAggregateVersion();
                base.AddEvent(e);
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
