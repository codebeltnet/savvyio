using Savvyio.Data;

namespace Savvyio.Queries
{
    public interface IQueryRepository<TProjection> : IPersistentDataAccessObject<TProjection> where TProjection : class
    {
    }
}
