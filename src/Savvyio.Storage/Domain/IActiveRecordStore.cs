using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public interface IActiveRecordStore<TAggregate, in TKey> where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
    {
        //Task<TAggregate> LoadAsync<TAggregate, TKey>(TKey id, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>;

        //Task<IQueryable<TAggregate>> QueryAsync<TAggregate, TKey>(Expression<Func<TAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>;

        //Task SaveAsync<TAggregate, TKey>(TAggregate aggregate, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>;

        //Task RemoveAsync<TAggregate, TKey>(TKey id, Action<AsyncOptions> setup = null) where TAggregate : class, IAggregateRoot<TKey>;

        Task<TAggregate> LoadAsync(TKey id, Action<AsyncOptions> setup = null);

        Task<IQueryable<TAggregate>> QueryAsync(Expression<Func<TAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null);

        Task SaveAsync(TAggregate aggregate, Action<AsyncOptions> setup = null);

        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);
    }
}
