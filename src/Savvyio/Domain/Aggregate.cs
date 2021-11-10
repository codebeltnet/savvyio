using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Represents the base class from which all implementations of an Aggregate Root (as specified in Domain Driven Design) should derive.
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <typeparam name="TEvent">The type of the event that implements the <see cref="IDomainEvent"/> interface.</typeparam>
    /// <seealso cref="Entity{TKey}" />
    /// <seealso cref="IAggregateNotification{TEvent}" />
    public abstract class Aggregate<TKey, TEvent> : Entity<TKey>, IAggregateNotification<TEvent> where TEvent : IDomainEvent
    {
        private readonly List<TEvent> _events = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregate{TKey, TEvent}"/> class.
        /// </summary>
        protected Aggregate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Aggregate{TKey, TEvent}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        protected Aggregate(TKey id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the events that was added to the Aggregate.
        /// </summary>
        /// <value>The events added to the Aggregate.</value>
        public IReadOnlyList<TEvent> Events => _events;

        /// <summary>
        /// Adds an event to the Aggregate.
        /// </summary>
        /// <param name="event">The event to be added to the end of <see cref="Events"/>.</param>
        protected void AddEvent(TEvent @event)
        {
            _events.Add(@event);
        }

        /// <summary>
        /// Removes all events from the Aggregate.
        /// </summary>
        public void RemoveAllEvents()
        {
            _events.Clear();
        }
    }
}
