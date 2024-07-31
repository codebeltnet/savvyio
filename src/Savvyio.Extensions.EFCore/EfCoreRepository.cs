using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentRepository{TEntity,TKey}"/> interface to serve as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IPersistentRepository{TEntity, TKey}" />
    public class EfCoreRepository<TEntity, TKey> : IPersistentRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        public EfCoreRepository(IEfCoreDataSource source)
        {
            Validator.ThrowIfNull(source);
            Set = source.Set<TEntity>();
        }

        /// <summary>
        /// Gets the associated <see cref="DbSet{TEntity}"/> for this repository.
        /// </summary>
        /// <value>The associated <see cref="DbSet{TEntity}"/> for this repository.</value>
        protected DbSet<TEntity> Set { get; }

        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public virtual void Add(TEntity entity)
        {
            Set.Add(entity);
        }

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
        }

        /// <summary>
        /// Loads the entity from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the entity.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the entity of the operation or <c>null</c> if not found.</returns>
        public virtual Task<TEntity> GetByIdAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return Set.FindAsync(new[] { id }, options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Finds all entities matching the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the entities to retrieve.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching entities of the operation or an empty sequence if no match was found.</returns>
        public virtual async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return await (predicate == null ? Set.ToListAsync(options.CancellationToken).ConfigureAwait(false) : Set.Where(predicate).ToListAsync(options.CancellationToken).ConfigureAwait(false));
        }

        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be removed from the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public virtual void Remove(TEntity entity)
        {
            Set.Remove(entity);
        }

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be removed from the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Set.RemoveRange(entities);
        }
    }
}
