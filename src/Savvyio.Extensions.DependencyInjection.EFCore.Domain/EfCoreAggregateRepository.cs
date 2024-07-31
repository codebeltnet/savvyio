using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreRepository{TEntity,TKey}"/> that is optimized for Domain Driven Design.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreRepository{TEntity,TKey}" />
    /// <seealso cref="IPersistentRepository{TEntity, TKey, TMarker}" />
    public class EfCoreAggregateRepository<TEntity, TKey, TMarker> : EfCoreRepository<TEntity, TKey>, IAggregateRepository<TEntity, TKey, TMarker> where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity,TKey,TEvent}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource{TMarker}"/> that handles actual I/O communication with a source of data.</param>
        public EfCoreAggregateRepository(IEfCoreDataSource<TMarker> source) : base(source)
        {
        }
    }
}
