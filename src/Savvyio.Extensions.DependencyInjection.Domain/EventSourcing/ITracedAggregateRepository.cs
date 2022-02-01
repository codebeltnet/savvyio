using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.Storage;

namespace Savvyio.Extensions.DependencyInjection.Domain.EventSourcing
{
    /// <summary>
    /// Defines a generic way to support multiple implementations traced read- and writable repositories (CRud) that is optimized for Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="ITracedAggregateRepository{TEntity,TKey}"/>
    /// <seealso cref="IReadableRepository{TEntity,TKey,TMarker}"/>
    /// <seealso cref="IWritableRepository{TEntity,TKey,TMarker}"/>
    public interface ITracedAggregateRepository<TEntity, TKey, TMarker> : ITracedAggregateRepository<TEntity, TKey>, IReadableRepository<TEntity, TKey, TMarker>, IWritableRepository<TEntity, TKey, TMarker> where TEntity : class, ITracedAggregateRoot<TKey>
    {
    }
}
