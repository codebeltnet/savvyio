using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;

namespace Savvyio.Storage
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T}"/> interface to serve as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="IPersistentDataAccessObject{T}" />
    public class EfCoreDataAccessObject<T> : IPersistentDataAccessObject<T> where T : class
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
        public async Task CreateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            _dbSet.Add(dto);
            await _uow.SaveChangesAsync(setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            _dbSet.Update(dto);
            await _uow.SaveChangesAsync(setup).ConfigureAwait(false);
        }

        /// <summary>
        /// Finds an object from the specified <paramref name="predicate"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="predicate">The predicate that matches the object to retrieve in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        public Task<T> ReadAsync(Expression<Func<T, bool>> predicate, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(predicate, nameof(predicate));
            var options = setup.Configure();
            return _dbSet.SingleOrDefaultAsync(predicate, options.CancellationToken);
        }
        
        /// <summary>
        /// Finds all objects matching the specified <paramref name="predicate"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="predicate">The predicate that matches the objects to retrieve in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public async Task<IEnumerable<T>> ReadAllAsync(Expression<Func<T, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return await (predicate == null ? _dbSet.ToListAsync(options.CancellationToken).ConfigureAwait(false) : _dbSet.Where(predicate).ToListAsync(options.CancellationToken).ConfigureAwait(false));
        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IEfCoreDataStore"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public async Task DeleteAsync(T dto, Action<AsyncOptions> setup = null)
        {
            _dbSet.Remove(dto);
            await _uow.SaveChangesAsync(setup).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T,TMarker}"/> interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication towards a data store using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDataAccessObject{T}" />
    /// <seealso cref="IPersistentDataAccessObject{T, TMarker}" />
    public class EfCoreDataAccessObject<T, TMarker> : EfCoreDataAccessObject<T>, IPersistentDataAccessObject<T, TMarker> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataAccessObject{T, TMarker}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore{TMarker}"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreDataAccessObject(IEfCoreDataStore<TMarker> dataStore) : base(dataStore)
        {
        }
    }
}
