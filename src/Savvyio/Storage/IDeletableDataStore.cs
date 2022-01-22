using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting the actual I/O communication with a data store that is responsible of deleting data (cruD).
    /// </summary>
    /// <seealso cref="IDataStore" />
    public interface IDeletableDataStore : IDataStore
    {
        /// <summary>
        /// Deletes the specified <paramref name="dto"/> from a data store asynchronous.
        /// </summary>
        /// <typeparam name="T">The type of the Entity/Projection/DTO you want to delete.</typeparam>
        /// <param name="dto">The object to delete from a data store.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task DeleteAsync<T>(T dto, Action<AsyncOptions> setup = null) where T : class;
    }
}
