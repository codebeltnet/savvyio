using Cuemon.Threading;
using Savvyio.Data;

namespace Savvyio.Queries
{
    public interface IQueryRepository<TProjection> : IPersistentDataAccessObject<TProjection, AsyncOptions> where TProjection : class
    {
    }
}
