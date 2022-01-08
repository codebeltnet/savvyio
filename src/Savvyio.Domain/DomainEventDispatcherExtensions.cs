using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="IDomainEventDispatcher"/> interface.
    /// </summary>
    public static class DomainEventDispatcherExtensions
    {
        /// <summary>
        /// Raises domain events from the specified <paramref name="aggregate"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <typeparam name="T">The type that implements the <see cref="IDomainEvent"/> interface.</typeparam>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> to extend.</param>
        /// <param name="aggregate">The aggregate to publish domain events from.</param>
        public static void RaiseMany<T>(this IDomainEventDispatcher dispatcher, IAggregateRoot<T> aggregate) where T : IDomainEvent
        {
            var events = aggregate.Events.ToList();
            if (!typeof(ITracedDomainEvent).IsAssignableFrom(typeof(T))) { aggregate.RemoveAllEvents(); }
            foreach (var @event in events)
            {
                dispatcher.Raise(@event.MergeMetadata(aggregate));
            }
        }

        /// <summary>
        /// Asynchronously raises domain events from the specified <paramref name="aggregate"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <typeparam name="T">The type that implements the <see cref="IDomainEvent"/> interface.</typeparam>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> to extend.</param>
        /// <param name="aggregate">The aggregate to publish domain events from.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task RaiseManyAsync<T>(this IDomainEventDispatcher dispatcher, IAggregateRoot<T> aggregate, Action<AsyncOptions> setup = null) where T : IDomainEvent
        {
            var events = aggregate.Events.ToList();
            if (!typeof(ITracedDomainEvent).IsAssignableFrom(typeof(T))) { aggregate.RemoveAllEvents(); }
            foreach (var @event in events)
            {
                await dispatcher.RaiseAsync(@event.MergeMetadata(aggregate), setup).ConfigureAwait(false);
            }
        }
    }
}
