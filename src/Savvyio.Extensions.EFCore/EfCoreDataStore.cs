using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Data;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Represents the base class from which all implementations of <see cref="EfCoreDataStore{T,TOptions}"/> should derive. This is an abstract class to serve as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of options associated with this DTO.</typeparam>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="IPersistentDataStore{T,TOptions}" />
    public abstract class EfCoreDataStore<T, TOptions> : IPersistentDataStore<T, TOptions>
        where T : class
        where TOptions : AsyncOptions, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStore{T, TOptions}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        protected EfCoreDataStore(IEfCoreDataSource source)
        {
            Validator.ThrowIfNull(source);
            DbSet = source.Set<T>();
            UnitOfWork = source;
        }

        /// <summary>
        /// Gets a <see cref="DbSet{TEntity}"/> that can be used to query and save instances of <typeparamref name="T"/>.
        /// </summary>
        /// <value>The <see cref="DbSet{TEntity}"/> that can be used to query and save instances of <typeparamref name="T"/>.</value>
        protected DbSet<T> DbSet { get; }

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
        public abstract Task CreateAsync(T dto, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task UpdateAsync(T dto, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Loads the object from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the object.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the object of the operation or <c>null</c> if not found.</returns>
        public abstract Task<T> GetByIdAsync(object id, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public abstract Task<IEnumerable<T>> FindAllAsync(Action<TOptions> setup = null);

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IEfCoreDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IEfCoreDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task DeleteAsync(T dto, Action<AsyncOptions> setup = null);
    }
}
