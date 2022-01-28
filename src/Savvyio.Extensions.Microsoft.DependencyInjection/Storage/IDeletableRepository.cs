using Savvyio.Storage;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.Storage
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of deletable repositories (cruD).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IRepository{TEntity, TKey, TMarker}" />
    /// <seealso cref="IDeletableRepository{TEntity, TKey}" />
    public interface IDeletableRepository<in TEntity, TKey, TMarker> : IRepository<TEntity, TKey, TMarker>, IDeletableRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
    }
}
