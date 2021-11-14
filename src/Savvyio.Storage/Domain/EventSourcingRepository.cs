using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Reflection;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public class EventSourcingRepository<TAggregate, TKey> : IEventSourcingRepository<TAggregate, TKey> where TAggregate : class, ITracedAggregateRoot<TKey>
    {
        private readonly IEventSourcingStore _store;
        private readonly IMediator _mediator;

        public EventSourcingRepository(IEventSourcingStore eventSourcingStore, IMediator mediator)
        {
            Validator.ThrowIfNull(eventSourcingStore, nameof(eventSourcingStore));
            _store = eventSourcingStore;
            _mediator = mediator;
        }

        public async Task<TAggregate> ReadAsync(TKey id, long fromVersion = 0, Action<AsyncOptions> setup = null)
        {
            var events = await _store.LoadStreamAsync(id, fromVersion, setup).ConfigureAwait(false);
            var list = events?.ToList();
            return list?.Count > 0 ? ActivatorFactory.CreateInstance<TKey, IEnumerable<IDomainEvent>, TAggregate>(id, list) : null;
        }

        public async Task SaveAsync(TAggregate aggregate, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(aggregate, nameof(aggregate));
            await _mediator.PublishDomainEventsAsync(aggregate, setup).ConfigureAwait(false);
            await _store.SaveStreamAsync(aggregate.Id, aggregate.Events, setup).ConfigureAwait(false);
        }
    }
}
