
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Extensions.EntityFrameworkCore;

namespace Savvyio.Extensions.DependencyInjection.EntityFrameworkCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreRepository{TEntity,TKey}"/> that is optimized for Domain Driven Design and <see cref="IAggregateRoot"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreRepository{TEntity,TKey}" />
    /// <seealso cref="IPersistentRepository{TEntity, TKey, TMarker}" />
    public class EfCoreAggregateRepository<TEntity, TKey, TMarker> : EfCoreRepository<TEntity, TKey>, IPersistentRepository<TEntity, TKey, TMarker> where TEntity : class, IEntity<TKey>, IAggregateRoot<IDomainEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity,TKey,TEvent}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreAggregateRepository(IEfCoreDataStore<TMarker> dataStore) : base(dataStore)
        {
        }
    }
}
