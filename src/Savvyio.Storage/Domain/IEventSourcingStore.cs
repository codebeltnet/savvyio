using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public interface IEventSourcingStore
    {
        Task<IEnumerable<ITracedDomainEvent>> LoadStreamAsync<TKey>(TKey id, long version = 0, Action<AsyncOptions> setup = null);

        Task SaveStreamAsync<TKey>(TKey id, IEnumerable<ITracedDomainEvent> events, Action<AsyncOptions> setup = null);
    }
}
