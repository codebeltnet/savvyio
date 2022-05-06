using Savvyio.Domain;

namespace Savvyio.Extensions.DependencyInjection.Domain
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of searchable repositories (cRud).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IRepository{TEntity, TKey, TMarker}" />
    /// <seealso cref="IReadableRepository{TEntity, TKey}" />
    public interface ISearchableRepository<TEntity, TKey, TMarker> : IRepository<TEntity, TKey, TMarker>, ISearchableRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
    }
}
