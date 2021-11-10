using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides a way to support Domain Events as specified in Domain Driven Design.
    /// </summary>
    /// <typeparam name="T">The type of the event that implements the <see cref="IDomainEvent"/> interface.</typeparam>
    public interface IAggregateNotification<out T> where T : IDomainEvent
    {
        /// <summary>
        /// Gets the events that was added to the Aggregate.
        /// </summary>
        /// <value>The events added to the Aggregate.</value>
        IReadOnlyList<T> Events { get; }

        /// <summary>
        /// Removes all events from the Aggregate.
        /// </summary>
        void RemoveAllEvents();
    }
}
