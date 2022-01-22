using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting readable data access (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IReadableDataAccessObject<T> : IDataAccessObject<T> where T : class
    {
        /// <summary>
        /// Finds an object from the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the object to retrieve.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching object of the operation or <c>null</c> if no match was found.</returns>
        Task<T> ReadAsync(Expression<Func<T, bool>> predicate, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Finds all objects matching the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the objects to retrieve.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching objects of the operation or an empty sequence if no match was found.</returns>
        Task<IEnumerable<T>> ReadAllAsync(Expression<Func<T, bool>> predicate = null, Action<AsyncOptions> setup = null);
    }
}
