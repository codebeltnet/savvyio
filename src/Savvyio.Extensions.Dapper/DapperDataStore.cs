using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using Savvyio.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Represents the base class from which all implementations of <see cref="DapperDataStore{T,TOptions}"/> should derive. This is an abstract class to serve as an abstraction layer before the actual I/O communication with a source of data using Dapper.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of options associated with this DTO.</typeparam>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="IPersistentDataStore{T,TOptions}" />
    public abstract class DapperDataStore<T, TOptions> : Disposable, IPersistentDataStore<T, TOptions>
        where T : class
        where TOptions : AsyncOptions, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataStore{T,TOptions}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IDapperDataSource"/> that handles actual I/O communication with a source of data.</param>
        protected DapperDataStore(IDapperDataSource source)
        {
            Source = source;
        }

        /// <summary>
        /// Gets the <see cref="IDapperDataSource"/> that handles actual I/O communication with a source of data.
        /// </summary>
        /// <value>The <see cref="IDapperDataSource"/> that handles actual I/O communication with a source of data.</value>
        protected IDapperDataSource Source { get; }

        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IDapperDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task CreateAsync(T dto, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IDapperDataSource"/>.</param>
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
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public abstract Task<IEnumerable<T>> FindAllAsync(Action<TOptions> setup = null);

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IDapperDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public abstract Task DeleteAsync(T dto, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Called when this object is being disposed by either <see cref="M:Cuemon.Disposable.Dispose" /> or <see cref="M:Cuemon.Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="P:Cuemon.Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            Source?.Dispose();
        }
    }
}
