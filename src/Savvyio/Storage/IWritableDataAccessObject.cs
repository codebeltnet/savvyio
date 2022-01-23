using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting writable data access objects (CrUd).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IWritableDataAccessObject<in T> : IDataAccessObject<T> where T : class
    {
        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous.
        /// </summary>
        /// <param name="dto">The object to create.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task CreateAsync(T dto, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous.
        /// </summary>
        /// <param name="dto">The object to update.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task UpdateAsync(T dto, Action<AsyncOptions> setup = null);
    }
}
