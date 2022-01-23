using Savvyio.Storage;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of persistent repositories (CRUD).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IRepository{TEntity, TKey, TMarker}" />
    /// <seealso cref="IPersistentRepository{TEntity, TKey}" />
    public interface IPersistentRepository<TEntity, TKey, TMarker> : IRepository<TEntity, TKey, TMarker>, IPersistentRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
    }
}
