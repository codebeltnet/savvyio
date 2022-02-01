using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Reflection;
using Cuemon.Threading;
using Savvyio.Domain;
using Savvyio.Domain.EventSourcing;
using Savvyio.Storage;

namespace Savvyio.Extensions.EntityFrameworkCore.Domain.EventSourcing
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreRepository{TEntity,TKey}"/> that is optimized for Domain Driven Design and Event Sourcing.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that implements the <see cref="ITracedAggregateRoot{TKey}"/> interface.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IReadableRepository{TEntity, TKey}" />
    /// <seealso cref="IWritableRepository{TEntity, TKey}" />
    public class EfCoreTracedAggregateRepository<TEntity, TKey> : IReadableRepository<TEntity, TKey>, IWritableRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>, ITracedAggregateRoot<TKey>
    {
        private readonly EfCoreRepository<EfCoreTracedAggregateEntity<TEntity, TKey>, TKey> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreTracedAggregateRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreTracedAggregateRepository(IEfCoreDataStore dataStore)
        {
            _repository = new EfCoreRepository<EfCoreTracedAggregateEntity<TEntity, TKey>, TKey>(dataStore);
        }

        /// <summary>
        /// Loads an aggregate from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the aggregate.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the entity of the operation or <c>null</c> if not found.</returns>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="TEntity"/> does not have a suitable constructor.
        /// </exception>
        public async Task<TEntity> GetByIdAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            var entities = await _repository.FindAllAsync(entity => entity.Id.Equals(id), setup).ConfigureAwait(false);
            var events = entities.OrderBy(entity => entity.Version).Select(entity => entity.ToTracedDomainEvent(Type.GetType(entity.Type)));
            try
            {
                return ActivatorFactory.CreateInstance<TKey, IEnumerable<ITracedDomainEvent>, TEntity>(id, events, o => o.Flags = new MemberReflection());
            }
            catch (MissingMethodException e)
            {
                throw new MissingMethodException($"Constructor on type '{typeof(TEntity).Name}' not found. Please add a private constructor with this signature: '{typeof(TEntity).Name}({typeof(TKey).Name}, {typeof(IEnumerable<ITracedDomainEvent>).ToFriendlyName()})'.", e);
            }
        }

        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The aggregate to add.</param>
        public void Add(TEntity entity)
        {
            foreach (var current in entity.Events)
            {
                _repository.Add(new EfCoreTracedAggregateEntity<TEntity, TKey>(entity, current));
            }
            entity.RemoveAllEvents();
        }

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The aggregates to add.</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }
    }
}
