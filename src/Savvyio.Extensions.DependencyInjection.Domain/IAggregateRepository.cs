using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Storage;

namespace Savvyio.Extensions.DependencyInjection.Domain
{
    /// <summary>
    /// Defines a generic way to support multiple implementations of persistent repositories (CRUD) that is optimized for Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="IAggregateRepository{TEntity,TKey}"/>
    /// <seealso cref="IPersistentRepository{TEntity,TKey,TMarker}"/>
    public interface IAggregateRepository<TEntity, TKey, TMarker> : IAggregateRepository<TEntity, TKey>, IPersistentRepository<TEntity, TKey, TMarker> where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
    {
    }
}
