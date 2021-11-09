using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Domain;

namespace Savvyio.Queries
{
    public interface IQueryStore
    {
        Task<TProjection> LoadAsync<TProjection, TKey>(TKey id, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>;

        Task<IQueryable<TProjection>> QueryAsync<TProjection, TKey>(Expression<Func<TProjection, bool>> predicate = null, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>;

        Task SaveAsync<TProjection, TKey>(TProjection projection, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>;

        Task RemoveAsync<TProjection, TKey>(TKey id, Action<AsyncOptions> setup = null) where TProjection : class, IIdentity<TKey>;
    }
}
