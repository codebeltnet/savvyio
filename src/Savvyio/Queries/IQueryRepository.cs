using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Domain;

namespace Savvyio.Queries
{
    public interface IQueryRepository<TProjection, in TKey> where TProjection : class, IIdentity<TKey>
    {
        Task<TProjection> LoadAsync(TKey id, Action<AsyncOptions> setup = null);

        Task<IQueryable<TProjection>> QueryAsync(Expression<Func<TProjection, bool>> predicate = null, Action<AsyncOptions> setup = null);

        Task SaveAsync(TProjection projection, Action<AsyncOptions> setup = null);

        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);
    }
}
