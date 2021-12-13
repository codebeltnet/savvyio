using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Domain;

namespace Savvyio.Queries
{
    public interface IQueryRepository<TProjection, in TKey> : IReadOnlyRepository<TProjection, TKey> where TProjection : class, IIdentity<TKey>
    {
        Task SaveAsync(TProjection projection, Action<AsyncOptions> setup = null);

        Task RemoveAsync(TKey id, Action<AsyncOptions> setup = null);
    }
}
