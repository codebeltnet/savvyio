using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Extension methods for the <see cref="IDomainEventDispatcher"/> interface.
    /// </summary>
    public static class DomainEventDispatcherExtensions
    {
        /// <summary>
        /// Raises domain events from the specified <paramref name="context"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> to extend.</param>
        /// <param name="context">The <see cref="DbContext"/> to extract aggregate(s) and publish domain events from.</param>
        public static void RaiseMany(this IDomainEventDispatcher dispatcher, DbContext context)
        {
            var entries = context.ChangeTracker.Entries<IAggregateRoot<IDomainEvent>>().Where(entry => entry.Entity.Events.Any()).ToList();
            foreach (var entry in entries)
            {
                var aggregate = entry.Entity;
                var events = entries.SelectMany(ee => ee.Entity.Events).ToList();
                var eventsType = events.First().GetType();
                if (!typeof(ITracedDomainEvent).IsAssignableFrom(eventsType)) { aggregate.RemoveAllEvents(); }
                foreach (var @event in events)
                {
                    dispatcher.Raise(@event.MergeMetadata(aggregate));
                }
            }
        }

        /// <summary>
        /// Asynchronously raises domain events from the specified <paramref name="context"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> to extend.</param>
        /// <param name="context">The <see cref="DbContext"/> to extract aggregate(s) and publish domain events from.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task RaiseManyAsync(this IDomainEventDispatcher dispatcher, DbContext context, Action<AsyncOptions> setup = null)
        {
            var entries = context.ChangeTracker.Entries<IAggregateRoot<IDomainEvent>>().Where(entry => entry.Entity.Events.Any()).ToList();
            foreach (var entry in entries)
            {
                var aggregate = entry.Entity;
                var events = entries.SelectMany(ee => ee.Entity.Events).ToList();
                var eventsType = events.First().GetType();
                if (!typeof(ITracedDomainEvent).IsAssignableFrom(eventsType)) { aggregate.RemoveAllEvents(); }
                foreach (var @event in events)
                {
                    await dispatcher.RaiseAsync(@event.MergeMetadata(aggregate), setup).ConfigureAwait(false);
                }
            }
        }
    }
}
