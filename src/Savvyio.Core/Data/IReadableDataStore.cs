using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Data
{

    /// <summary>
    /// Defines a generic way of abstracting readable data access objects (cRud).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IReadableDataStore<T> : IDataStore<T> where T : class

    {
        /// <summary>
        /// Loads the object from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the object.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the object of the operation or <c>null</c> if not found.</returns>
        Task<T> GetByIdAsync(object id, Action<AsyncOptions> setup = null);
    }
}
