using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// Defines a generic way of abstracting deletable data access objects (cruD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <seealso cref="IDataAccessObject{T}"/>
    public interface IDeletableDataAccessObject<in T> : IDataAccessObject<T> where T : class
    {
        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous.
        /// </summary>
        /// <param name="dto">The object to delete.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task DeleteAsync(T dto, Action<AsyncOptions> setup = null);
    }
}
