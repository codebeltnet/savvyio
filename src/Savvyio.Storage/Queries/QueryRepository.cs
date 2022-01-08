using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using Savvyio.Domain;

namespace Savvyio.Queries
{
    //public class QueryRepository<TProjection, TKey> : IQueryRepository<TProjection, TKey> where TProjection : class, IIdentity<TKey>
    //{
    //    private readonly IQueryStore  _store;

    //    public QueryRepository(IQueryStore queryStore)
    //    {
    //        Validator.ThrowIfNull(queryStore, nameof(queryStore));
    //        _store = queryStore;
    //    }

    //    public Task<TProjection> LoadAsync(TKey id, Action<AsyncOptions> setup = null)
    //    {
    //        return _store.LoadAsync<TProjection, TKey>(id, setup);
    //    }

    //    public Task<IQueryable<TProjection>> QueryAsync(Expression<Func<TProjection, bool>> predicate = null, Action<AsyncOptions> setup = null)
    //    {
    //        return _store.QueryAsync<TProjection, TKey>(predicate, setup);
    //    }

    //    public Task SaveAsync(TProjection projection, Action<AsyncOptions> setup = null)
    //    {
    //        Validator.ThrowIfNull(projection, nameof(projection));
    //        return _store.SaveAsync<TProjection, TKey>(projection, setup);
    //    }

    //    public Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null)
    //    {
    //        return _store.RemoveAsync<TProjection, TKey>(id, setup);
    //    }
    //}
}
