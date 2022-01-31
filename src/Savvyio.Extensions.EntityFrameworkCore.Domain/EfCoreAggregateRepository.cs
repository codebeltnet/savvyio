using Savvyio.Domain;

namespace Savvyio.Extensions.EntityFrameworkCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreRepository{TEntity,TKey}"/> that is optimized for Domain Driven Design and <see cref="IAggregateRoot{TEvent}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="EfCoreRepository{TEntity,TKey}" />
    public class EfCoreAggregateRepository<TEntity, TKey> : EfCoreRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, IAggregateRoot<IDomainEvent>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity,TKey,TEvent}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreAggregateRepository(IEfCoreDataStore dataStore) : base(dataStore)
        {
        }
    }
}
