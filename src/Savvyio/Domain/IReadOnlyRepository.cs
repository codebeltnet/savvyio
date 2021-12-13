using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    public interface IReadOnlyRepository<TModel, in TKey> where TModel : class, IIdentity<TKey>
    {
        Task<TModel> LoadAsync(TKey id, Action<AsyncOptions> setup = null);

        Task<IQueryable<TModel>> QueryAsync(Expression<Func<TModel, bool>> predicate = null, Action<AsyncOptions> setup = null);
    }
}
