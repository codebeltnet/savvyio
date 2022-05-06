using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Data
{
    /// <summary>
    /// Defines a generic way of abstracting deletable data access objects (cruD).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
    /// <seealso cref="IDataAccessObject{T,TOptions}"/>
    public interface IDeletableDataAccessObject<in T, TOptions> : IDataAccessObject<T, TOptions>
        where T : class 
        where TOptions : AsyncOptions, new()
    {
        /// <summary>
        /// Deletes the specified <paramref name="dto"/> asynchronous.
        /// </summary>
        /// <param name="dto">The object to delete.</param>
        /// <param name="setup">The <typeparamref name="TOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        Task DeleteAsync(T dto, Action<TOptions> setup = null);
    }
}
