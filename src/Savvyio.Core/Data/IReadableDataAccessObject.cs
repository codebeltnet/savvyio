using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Data
{

    /// <summary>
    /// Defines a generic way of abstracting readable data access objects (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <seealso cref="IDataAccessObject{T,TOptions}"/>
    public interface IReadableDataAccessObject<T, TOptions> : IDataAccessObject<T, TOptions> 
        where T : class 
        where TOptions : AsyncOptions, new()
    {
        /// <summary>
        /// Finds an object from the specified <paramref name="setup"/> asynchronous.
        /// </summary>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        Task<T> ReadAsync(Action<TOptions> setup = null);

        /// <summary>
        /// Finds all objects matching the specified <paramref name="setup"/> asynchronous.
        /// </summary>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        Task<IEnumerable<T>> ReadAllAsync(Action<TOptions> setup = null);
    }
}
