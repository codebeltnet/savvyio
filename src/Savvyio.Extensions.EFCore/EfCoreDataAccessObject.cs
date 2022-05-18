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
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T,TOptions}"/> interface to serve as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions}" />
    public class EfCoreDataAccessObject<T> : IPersistentDataAccessObject<T, EfCoreOptions<T>> where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataAccessObject{T}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreDataAccessObject(IEfCoreDataStore dataStore)
        {
            _dbSet = dataStore.Set<T>();
            _uow = dataStore;
        }


        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task CreateAsync(T dto, Action<EfCoreOptions<T>> setup = null)
        {
            _dbSet.Add(dto);
            await _uow.SaveChangesAsync(Patterns.ConfigureExchange<EfCoreOptions<T>, AsyncOptions>(setup)).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task UpdateAsync(T dto, Action<EfCoreOptions<T>> setup = null)
        {
            _dbSet.Update(dto);
            await _uow.SaveChangesAsync(Patterns.ConfigureExchange<EfCoreOptions<T>, AsyncOptions>(setup)).ConfigureAwait(false);
        }

        /// <summary>
        /// Finds an object from the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        public Task<T> ReadAsync(Action<EfCoreOptions<T>> setup)
        {
            Validator.ThrowIfNull(setup, nameof(setup));
            var options = Validator.ThrowIf.InvalidState(setup.Configure(), o =>
            {
                Validator.ThrowIfNull(o.Predicate, nameof(o.Predicate));
            });

            return _dbSet.SingleOrDefaultAsync(options.Predicate, options.CancellationToken);
        }
        
        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public async Task<IEnumerable<T>> ReadAllAsync(Action<EfCoreOptions<T>> setup = null)
        {
            var options = setup.Configure();
            return await (options.Predicate == null ? _dbSet.ToListAsync(options.CancellationToken).ConfigureAwait(false) : _dbSet.Where(options.Predicate).ToListAsync(options.CancellationToken).ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task DeleteAsync(T dto, Action<EfCoreOptions<T>> setup = null)
        {
            _dbSet.Remove(dto);
            await _uow.SaveChangesAsync(Patterns.ConfigureExchange<EfCoreOptions<T>, AsyncOptions>(setup)).ConfigureAwait(false);
        }
    }
}
