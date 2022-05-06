using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// Defines a generic way of abstracting writable data access objects (CrUd).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <seealso cref="IDataAccessObject{T,TOptions}"/>
    public interface IWritableDataAccessObject<in T, TOptions> : IDataAccessObject<T, TOptions> 
        where T : class
        where TOptions : AsyncOptions, new()
    {
        /// <summary>
        /// Creates the specified <paramref name="dto"/> asynchronous.
        /// </summary>
        /// <param name="dto">The object to create.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task CreateAsync(T dto, Action<TOptions> setup = null);

        /// <summary>
        /// Updates the specified <paramref name="dto"/> asynchronous.
        /// </summary>
        /// <param name="dto">The object to update.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task UpdateAsync(T dto, Action<TOptions> setup = null);
    }
}
