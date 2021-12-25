using System;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Domain;
using Savvyio.Events;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="IMediator"/> interface.
    /// </summary>
    public static class MediatorExtensions
    {
        /// <summary>
        /// Publishes the specified <paramref name="event"/>.
        /// </summary>
        /// <param name="mediator">The <see cref="IMediator"/> to extend.</param>
        /// <param name="event">The <see cref="IDomainEvent"/> to publish.</param>
        public static void PublishDomainEvent(this IMediator mediator, IDomainEvent @event)
        {
            mediator.Publish(@event);
        }


        /// <summary>
        /// Publishes the specified <paramref name="event"/> asynchronous.
        /// </summary>
        /// <param name="mediator">The <see cref="IMediator"/> to extend.</param>
        /// <param name="event">The <see cref="IDomainEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task PublishDomainEventAsync(this IMediator mediator, IDomainEvent @event, Action<AsyncOptions> setup = null)
        {
            return mediator.PublishAsync(@event, setup);
        }

        /// <summary>
        /// Publishes the specified <paramref name="event"/>.
        /// </summary>
        /// <param name="mediator">The <see cref="IMediator"/> to extend.</param>
        /// <param name="event">The <see cref="IIntegrationEvent"/> to publish.</param>
        public static void PublishIntegrationEvent(this IMediator mediator, IIntegrationEvent @event)
        {
            mediator.Publish(@event);
        }

        /// <summary>
        /// Publishes the specified <paramref name="event"/> asynchronous.
        /// </summary>
        /// <param name="mediator">The <see cref="IMediator"/> to extend.</param>
        /// <param name="event">The <see cref="IIntegrationEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task PublishIntegrationEventAsync(this IMediator mediator, IIntegrationEvent @event, Action<AsyncOptions> setup = null)
        {
            return mediator.PublishAsync(@event, setup);
        }

        /// <summary>
        /// Publish domain events from the specified <paramref name="aggregate"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <typeparam name="T">The type that implements the <see cref="IDomainEvent"/> interface.</typeparam>
        /// <param name="mediator">The <see cref="IMediator"/> to extend.</param>
        /// <param name="aggregate">The aggregate to publish domain events from.</param>
        public static void PublishDomainEvents<T>(this IMediator mediator, IAggregateRoot<T> aggregate) where T : IDomainEvent
        {
            var events = aggregate.Events.ToList();
            if (!typeof(ITracedDomainEvent).IsAssignableFrom(typeof(T))) { aggregate.RemoveAllEvents(); }
            foreach (var @event in events)
            {
                mediator.Publish(@event.MergeMetadata(aggregate));
            }
        }

        /// <summary>
        /// Asynchronously publish domain events from the specified <paramref name="aggregate"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <typeparam name="T">The type that implements the <see cref="IDomainEvent"/> interface.</typeparam>
        /// <param name="mediator">The <see cref="IMediator"/> to extend.</param>
        /// <param name="aggregate">The aggregate to publish domain events from.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task PublishDomainEventsAsync<T>(this IMediator mediator, IAggregateRoot<T> aggregate, Action<AsyncOptions> setup = null) where T : IDomainEvent
        {
            var events = aggregate.Events.ToList();
            if (!typeof(ITracedDomainEvent).IsAssignableFrom(typeof(T))) { aggregate.RemoveAllEvents(); }
            foreach (var @event in events)
            {
                await mediator.PublishAsync(@event.MergeMetadata(aggregate), setup).ConfigureAwait(false);
            }
        }
    }
}
