using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Data;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataStore{T,TOptions}"/> interface to serve as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="IPersistentDataStore{T,TOptions}" />
    public class EfCoreDataStore<T> : IPersistentDataStore<T, EfCoreQueryOptions<T>> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStore{T}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        public EfCoreDataStore(IEfCoreDataSource source)
        {
            Validator.ThrowIfNull(source);
            Set = source.Set<T>();
            UnitOfWork = source;
        }

        /// <summary>
        /// Gets a <see cref="DbSet{TEntity}"/> that can be used to query and save instances of <typeparamref name="T"/>.
        /// </summary>
        /// <value>The <see cref="DbSet{TEntity}"/> that can be used to query and save instances of <typeparamref name="T"/>.</value>
        protected DbSet<T> Set { get; }

        /// <summary>
        /// Gets a <see cref="IUnitOfWork"/> that can be used to save instances of <typeparamref name="T"/>.
        /// </summary>
        /// <value>The <see cref="IUnitOfWork"/> that can be used to save instances of <typeparamref name="T"/>.</value>
        protected IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual async Task CreateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            Set.Add(dto);
            await UnitOfWork.SaveChangesAsync(setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual async Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            Set.Update(dto);
            await UnitOfWork.SaveChangesAsync(setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads the object from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the object.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the object of the operation or <c>null</c> if not found.</returns>
        public virtual Task<T> GetByIdAsync(object id, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return Set.FindAsync(new[] { id }, options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreQueryOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public virtual async Task<IEnumerable<T>> FindAllAsync(Action<EfCoreQueryOptions<T>> setup = null)
        {
            var options = setup.Configure();
            return await (options.Predicate == null ? Set.ToListAsync(options.CancellationToken).ConfigureAwait(false) : Set.Where(options.Predicate).ToListAsync(options.CancellationToken).ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual async Task DeleteAsync(T dto, Action<AsyncOptions> setup = null)
        {
            Set.Remove(dto);
            await UnitOfWork.SaveChangesAsync(setup).ConfigureAwait(false);
        }
    }
}
