using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Domain;

namespace Savvyio.Queries
{
    public class InMemoryQueryStore : IQueryStore
    {
        private readonly List<object> _store = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryQueryStore"/> class.
        /// </summary>
        public InMemoryQueryStore()
        {
        }

        public Task<TProjection> LoadAsync<TProjection, TKey>(TKey id, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>
        {
            return Task.FromResult(_store.SingleOrDefault(o => o.As<IAggregateRoot<IDomainEvent, TKey>>().Id.Equals(id)) as TProjection);
        }

        public Task<IQueryable<TProjection>> QueryAsync<TProjection, TKey>(Expression<Func<TProjection, bool>> predicate, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>
        {
            return Task.FromResult(Condition.TernaryIf(predicate == null, () => _store.Cast<TProjection>().AsQueryable(), () =>
            {
                var funcPredicate = predicate.Compile();
                return _store.Where(o => funcPredicate((TProjection)o)).Cast<TProjection>().AsQueryable();
            }));
        }

        public async Task SaveAsync<TProjection, TKey>(TProjection projection, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>
        {
            var entity = await LoadAsync<TProjection, TKey>(projection.Id);
            if (entity != null)
            {
                var index = _store.IndexOf(entity);
                if (index != -1)
                {
                    _store[index] = entity;
                }
            }
            else
            {
                _store.Add(projection);
            }
        }

        public async Task RemoveAsync<TProjection, TKey>(TKey id, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>
        {
            var entity = await LoadAsync<TProjection, TKey>(id);
            if (entity != null)
            {
                var index = _store.IndexOf(entity);
                if (index != -1)
                {
                    _store.RemoveAt(index);
                }
            }
        }
    }
}
