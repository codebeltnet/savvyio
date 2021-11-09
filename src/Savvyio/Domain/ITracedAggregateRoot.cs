﻿using System.Collections.Generic;

namespace Savvyio.Domain
{
    /// <summary>
    /// Provides an Event Sourcing capable contract of an Aggregate as specified in Domain Driven Design.
    /// Implements the <see cref="IAggregateRoot{TKey}" />
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="IAggregateRoot{TKey}" />
    public interface ITracedAggregateRoot<out TKey> : IAggregateRoot, IEntity<TKey>, IAggregateNotification<ITracedDomainEvent>
    {
        long Version { get; }

        void ReplayEvents(IEnumerable<ITracedDomainEvent> events);

        //void RaiseEvent(ITracedDomainEvent @event);
    }
}
