using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Dapper;
using Savvyio.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentDataAccessObject{T,TOptions}"/> interface to serve as an abstraction layer before the actual I/O communication towards a data store using Dapper.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions}" />
    public class DapperDataAccessObject<T> : Disposable, IPersistentDataAccessObject<T, DapperOptions> where T : class
    {
        private readonly IDapperDataStore _dataStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataAccessObject{T}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IDapperDataStore"/> that handles actual I/O communication towards a data store.</param>
        public DapperDataAccessObject(IDapperDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IDapperDataStore"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual Task CreateAsync(T dto, Action<DapperOptions> setup)
        {
            return CommonAsync(dto, setup);
        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IDapperDataStore"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual Task DeleteAsync(T dto, Action<DapperOptions> setup)
        {
            return CommonAsync(dto, setup);
        }

        /// <summary>
        /// Finds all objects matching the specified <paramref name="predicate"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="predicate">The optional predicate that matches the objects to retrieve in the associated <seealso cref="IDapperDataStore"/>. Pass in <c>null</c> to control fully by <paramref name="setup"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public virtual async Task<IEnumerable<T>> ReadAllAsync(Expression<Func<T, bool>> predicate, Action<DapperOptions> setup)
        {
            var cd = setup.Configure();
            var result = await _dataStore.QueryAsync<T>(cd).ConfigureAwait(false);
            return predicate == null ? result : result.Where(predicate.Compile());
        }
        
        /// <summary>
        /// Finds an object from the specified <paramref name="predicate"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="predicate">The optional predicate that matches the object to retrieve in the associated <seealso cref="IDapperDataStore"/>. Pass in <c>null</c> to control fully by <paramref name="setup"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        public virtual async Task<T> ReadAsync(Expression<Func<T, bool>> predicate, Action<DapperOptions> setup)
        {
            var result = await ReadAllAsync(predicate, setup).ConfigureAwait(false);
            return result.SingleOrDefault();
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IDapperDataStore"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public virtual Task UpdateAsync(T dto, Action<DapperOptions> setup)
        {
            return CommonAsync(dto, setup);
        }

        private Task CommonAsync(T dto, Action<DapperOptions> setup)
        {
            var cd = setup.Configure();
            cd.Parameters ??= dto;
            return _dataStore.ExecuteAsync(cd);
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="M:Cuemon.Disposable.Dispose" /> or <see cref="M:Cuemon.Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="P:Cuemon.Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            _dataStore?.Dispose();
        }
    }
}
