using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreRepository{TEntity,TKey}"/> that is optimized for Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that implements the <see cref="IAggregateRoot{TEvent}"/> interface.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="EfCoreRepository{TEntity,TKey}" />
    public class EfCoreAggregateRepository<TEntity, TKey> : EfCoreRepository<TEntity, TKey>, IAggregateRepository<TEntity, TKey> where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity,TKey}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        public EfCoreAggregateRepository(IEfCoreDataSource source) : base(source)
        {
        }
    }
}
