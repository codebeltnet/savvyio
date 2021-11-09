using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Domain;

namespace Savvyio
{
    public static class MediatorExtensions
    {
        public static void PublishDomainEvents<T>(this IMediator mediator, IAggregateNotification<T> aggregate) where T : IDomainEvent
        {
            var events = aggregate.Events.ToList();
            if (!typeof(ITracedDomainEvent).IsAssignableFrom(typeof(T))) { aggregate.RemoveAllEvents(); }
            foreach (var @event in events)
            {
                mediator.Publish(@event);
            }
        }

        public static async Task PublishDomainEventsAsync<T>(this IMediator mediator, IAggregateNotification<T> aggregate, Action<AsyncOptions> setup = null) where T : IDomainEvent
        {
            var events = aggregate.Events.ToList();
            if (!typeof(ITracedDomainEvent).IsAssignableFrom(typeof(T))) { aggregate.RemoveAllEvents(); }
            foreach (var @event in events)
            {
                await mediator.PublishAsync(@event, setup).ConfigureAwait(false);
            }
        }
    }
}
