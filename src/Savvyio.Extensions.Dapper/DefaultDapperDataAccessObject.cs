using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Dapper;
using Savvyio.Data;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Provides a default implementation of the <see cref="DapperDataAccessObject{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="IPersistentDataAccessObject{T,TOptions}" />
    public class DefaultDapperDataAccessObject<T> : DapperDataAccessObject<T> where T : class
    {
        private readonly IDapperDataStore _dataStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDapperDataAccessObject{T}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IDapperDataStore"/> that handles actual I/O communication towards a data store.</param>
        public DefaultDapperDataAccessObject(IDapperDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IDapperDataStore"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override Task CreateAsync(T dto, Action<DapperOptions> setup)
        {
            return CommonAsync(dto, setup);
        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IDapperDataStore"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override Task DeleteAsync(T dto, Action<DapperOptions> setup)
        {
            return CommonAsync(dto, setup);
        }

        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="setup">The <see cref="DapperOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public override async Task<IEnumerable<T>> ReadAllAsync(Action<DapperOptions> setup)
        {
            var cd = setup.Configure();
            return await _dataStore.QueryAsync<T>(cd).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Finds an object from the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="setup">The <see cref="DapperOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        public override async Task<T> ReadAsync(Action<DapperOptions> setup)
        {
            var result = await ReadAllAsync(setup).ConfigureAwait(false);
            return result.SingleOrDefault();
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataStore"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IDapperDataStore"/>.</param>
        /// <param name="setup">The <see cref="DapperOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override Task UpdateAsync(T dto, Action<DapperOptions> setup)
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
