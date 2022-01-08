using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public class ActiveRecordRepository<TAggregate, TKey> : IActiveRecordRepository<TAggregate, TKey> where TAggregate : class, IAggregateRoot<IDomainEvent, TKey>
    {
        private readonly IActiveRecordStore<TAggregate, TKey> _store;
        private readonly IDomainEventDispatcher _dispatcher;

        public ActiveRecordRepository(IActiveRecordStore<TAggregate, TKey> activeRecordStore, IDomainEventDispatcher dispatcher)
        {
            Validator.ThrowIfNull(activeRecordStore, nameof(activeRecordStore));
            Validator.ThrowIfNull(dispatcher, nameof(dispatcher));
            _store = activeRecordStore;
            _dispatcher = dispatcher;
        }

        public Task<TAggregate> LoadAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            return _store.LoadAsync(id, setup);
        }

        public Task<IQueryable<TAggregate>> QueryAsync(Expression<Func<TAggregate, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            return _store.QueryAsync(predicate, setup);
        }

        public async Task SaveAsync(TAggregate aggregate, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(aggregate, nameof(aggregate));
            await _dispatcher.RaiseManyAsync(aggregate, setup).ConfigureAwait(false);
            await _store.SaveAsync(aggregate, setup).ConfigureAwait(false);
        }

        public Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            return _store.RemoveAsync(id, setup);
        }
    }
}
