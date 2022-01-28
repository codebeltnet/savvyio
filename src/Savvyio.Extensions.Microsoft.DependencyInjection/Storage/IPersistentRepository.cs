using Savvyio.Storage;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of persistent repositories (CRUD).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IWritableRepository{TEntity,TKey,TMarker}"/>
    /// <seealso cref="IReadableRepository{TEntity,TKey,TMarker}"/>
    /// <seealso cref="IDeletableRepository{TEntity,TKey,TMarker}"/>
    public interface IPersistentRepository<TEntity, TKey, TMarker> : IPersistentRepository<TEntity, TKey>, IWritableRepository<TEntity, TKey, TMarker>, IReadableRepository<TEntity, TKey, TMarker>, IDeletableRepository<TEntity, TKey, TMarker> where TEntity : class, IIdentity<TKey>
    {
    }
}
