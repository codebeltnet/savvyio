using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Reflection;
using Cuemon.Threading;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Domain
{
    public class EventSourcingRepository<TAggregate, TKey> : IEventSourcingRepository<TAggregate, TKey> where TAggregate : class, ITracedAggregateRoot<TKey>
    {
        private readonly IEventSourcingStore _store;
        private readonly IDomainEventDispatcher _dispatcher;

        public EventSourcingRepository(IEventSourcingStore eventSourcingStore, IDomainEventDispatcher dispatcher)
        {
            Validator.ThrowIfNull(eventSourcingStore, nameof(eventSourcingStore));
            Validator.ThrowIfNull(dispatcher, nameof(dispatcher));
            _store = eventSourcingStore;
            _dispatcher = dispatcher;
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
            await _dispatcher.RaiseManyAsync(aggregate, setup).ConfigureAwait(false);
            await _store.SaveStreamAsync(aggregate.Id, aggregate.Events, setup).ConfigureAwait(false);
        }
    }
}
