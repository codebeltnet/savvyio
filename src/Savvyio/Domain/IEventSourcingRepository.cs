using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public interface IEventSourcingRepository<TAggregate, in TKey> : IPersistentRepository<TAggregate> where TAggregate : class, ITracedAggregateRoot<TKey>
    {
        Task<TAggregate> ReadAsync(TKey id, long fromVersion = 0, Action<AsyncOptions> setup = null);
    }
}
