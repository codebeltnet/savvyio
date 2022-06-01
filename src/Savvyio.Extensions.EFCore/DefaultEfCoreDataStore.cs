using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="EfCoreDataStore{T,TOptions}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="EfCoreDataStore{T,TOptions}" />
    public class DefaultEfCoreDataStore<T> : EfCoreDataStore<T, EfCoreQueryOptions<T>> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEfCoreDataStore{T}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        public DefaultEfCoreDataStore(IEfCoreDataSource source) : base(source)
        {
        }

        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override async Task CreateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            DbSet.Add(dto);
            await UnitOfWork.SaveChangesAsync(setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override async Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            DbSet.Update(dto);
            await UnitOfWork.SaveChangesAsync(setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads the object from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the object.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the object of the operation or <c>null</c> if not found.</returns>
        public override Task<T> GetByIdAsync(object id, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return DbSet.FindAsync(new[] { id }, options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreQueryOptions{T}"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public override async Task<IEnumerable<T>> FindAllAsync(Action<EfCoreQueryOptions<T>> setup = null)
        {
            var options = setup.Configure();
            var dbValues = await (options.Predicate == null ? DbSet.ToListAsync(options.CancellationToken).ConfigureAwait(false) : DbSet.Where(options.Predicate).ToListAsync(options.CancellationToken).ConfigureAwait(false));
            return dbValues.Any()
                ? dbValues
                : options.Predicate == null ? DbSet.Local.ToList() : DbSet.Local.Where(options.Predicate.Compile()).ToList();
        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override async Task DeleteAsync(T dto, Action<AsyncOptions> setup = null)
        {
            DbSet.Remove(dto);
            await UnitOfWork.SaveChangesAsync(setup).ConfigureAwait(false);
        }
    }
}
