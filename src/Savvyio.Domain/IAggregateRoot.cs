using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a marker interface of an Aggregate as specified in Domain Driven Design.
    /// </summary>
    public interface IAggregateRoot : IMetadata
    {
    }

    /// <summary>
    /// Defines an Event based contract of an Aggregate as specified in Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event that implements the <see cref="IDomainEvent"/> interface.</typeparam>
    /// <seealso cref="IAggregateRoot" />
    public interface IAggregateRoot<out TEvent> : IAggregateRoot
    {
        /// <summary>
        /// Gets the events that was added to the Aggregate.
        /// </summary>
        /// <value>The events added to the Aggregate.</value>
        IReadOnlyList<TEvent> Events { get; }

        /// <summary>
        /// Removes all events from the Aggregate.
        /// </summary>
        void RemoveAllEvents();
    }

    /// <summary>
    /// Defines an Event and Entity based contract of an Aggregate as specified in Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEvent">The type of the event that implements the <see cref="IDomainEvent"/> interface.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this Aggregate.</typeparam>
    /// <seealso cref="IAggregateRoot{TEvent}" />
    /// <seealso cref="IEntity{TKey}" />
    public interface IAggregateRoot<out TEvent, out TKey> : IAggregateRoot<TEvent>, IEntity<TKey>
    {
    }
}
