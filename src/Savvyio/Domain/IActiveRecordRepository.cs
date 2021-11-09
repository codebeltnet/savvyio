using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public interface IActiveRecordRepository<TAggregate, in TKey> : IPersistentRepository<TAggregate> where TAggregate : class, IAggregateRoot<TKey>
    {
        Task<TAggregate> LoadAsync(TKey id, Action<AsyncOptions> setup = null);

        Task<IQueryable<TAggregate>> QueryAsync(Expression<Func<TAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null);

        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);
        
    }
}
