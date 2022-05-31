using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using DapperExtensions;
using Savvyio.Extensions.Dapper;

namespace Savvyio.Extensions.DapperExtensions
{
    /// <summary>
    /// Provides a default implementation of the <see cref="DapperDataStore{T,TOptions}"/> class that is tailored for Plain Old CLR Objects (POCO) usage by DapperExtensions.
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="DapperDataStore{T,TOptions}" />
    public class DapperExtensionsDataStore<T> : DapperDataStore<T, DapperExtensionsQueryOptions<T>> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DapperExtensionsDataStore{T}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IDapperDataSource"/> that handles actual I/O communication with a source of data.</param>
        public DapperExtensionsDataStore(IDapperDataSource source) : base(source)
        {
        }

        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to create in the associated <seealso cref="IDapperDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override Task CreateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            return Source.InsertAsync(dto);

        }

        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to delete in the associated <seealso cref="IDapperDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override Task DeleteAsync(T dto, Action<AsyncOptions> setup = null)
        {
            return Source.DeleteAsync(dto);
        }

        /// <summary>
        /// Loads the object from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the object.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the object of the operation or <c>null</c> if not found.</returns>
        public override Task<T> GetByIdAsync(object id, Action<AsyncOptions> setup = null)
        {
            return Source.GetAsync<T>(id);
        }

        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="setup">The <see cref="DapperQueryOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        public override Task<IEnumerable<T>> FindAllAsync(Action<DapperExtensionsQueryOptions<T>> setup = null)
        {
            var options = setup.Configure();
            return options.Predicate == null ? Source.GetListAsync<T>() : Source.GetListAsync<T>(Predicates.Field(options.Predicate, options.Op, options.Value, options.Not, options.UseColumnPrefix, options.Function, options.FunctionParameters));
        }

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous in the associated <seealso cref="IDapperDataSource"/>.
        /// </summary>
        /// <param name="dto">The object to update in the associated <seealso cref="IDapperDataSource"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> that needs to be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public override Task UpdateAsync(T dto, Action<AsyncOptions> setup = null)
        {
            return Source.UpdateAsync(dto);
        }
    }
}
