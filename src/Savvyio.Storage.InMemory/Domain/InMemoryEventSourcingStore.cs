using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public class InMemoryEventSourcingStore : IEventSourcingStore
    {
        private readonly ConcurrentDictionary<object, List<ITracedDomainEvent>> _store = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryEventSourcingStore"/> class.
        /// </summary>
        public InMemoryEventSourcingStore()
        {
        }

        public Task<IEnumerable<ITracedDomainEvent>> LoadStreamAsync<TKey>(TKey id, long version = 0, Action<AsyncOptions> setup = null)
        {
            if (_store.TryGetValue(id, out var events))
            {
                return Task.FromResult(events.Where(de => de.Version >= version));
            }
            return Task.FromResult(Enumerable.Empty<ITracedDomainEvent>());
        }

        public Task SaveStreamAsync<TKey>(TKey id, IEnumerable<ITracedDomainEvent> events, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(events, nameof(events));
            if (_store.TryGetValue(id, out _))
            {
                _store[id].AddRange(events);
            }
            else
            {
                _store.TryAdd(id, new List<ITracedDomainEvent>(events));
            }
            return Task.CompletedTask;
        }
    }
}
