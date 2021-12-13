using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public interface IActiveRecordRepository<TAggregate, in TKey> : IPersistentRepository<TAggregate>, IReadOnlyRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
    {
        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);
        
    }
}
