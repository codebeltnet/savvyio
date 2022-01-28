using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting the actual I/O communication with a data store that is responsible of writing data (CrUd).
    /// </summary>
    /// <seealso cref="IDataStore" />
    public interface IWritableDataStore : IDataStore
    {
        /// <summary>
        /// Creates the specified <paramref name="dto"/> in a data store asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the Entity/Projection/DTO you want to create.</typeparam>
        /// <param name="dto">The object to create in a data store.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task CreateAsync<T>(T dto, Action<AsyncOptions> setup = null) where T : class;

        /// <summary>
        /// Updates the specified <paramref name="dto"/> in a data store asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the Entity/Projection/DTO you want to update.</typeparam>
        /// <param name="dto">The object to update in a data store.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task UpdateAsync<T>(T dto, Action<AsyncOptions> setup = null) where T : class;
    }
}
