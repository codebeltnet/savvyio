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
        /// Finds an object from the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the object to retrieve.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        Task<T> ReadAsync(Expression<Func<T, bool>> predicate, Action<TOptions> setup = null);

        /// <summary>
        /// Finds all objects matching the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the objects to retrieve.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        Task<IEnumerable<T>> ReadAllAsync(Expression<Func<T, bool>> predicate = null, Action<TOptions> setup = null);
    }
}
