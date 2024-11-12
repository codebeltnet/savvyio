using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// Defines a generic way of abstracting searchable data access objects (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <seealso cref="IDataStore{T}"/>
    public interface ISearchableDataStore<T, out TOptions> : IDataStore<T>
        where T : class
        where TOptions : AsyncOptions, new()
    {
        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous.
        /// </summary>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        Task<IEnumerable<T>> FindAllAsync(Action<TOptions> setup = null);
    }
}
