using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting writable data access (CrUd).
    /// </summary>
    /// <typeparam name="T">The type of the DTO.</typeparam>
    public interface IWritableDataAccessObject<in T> : IDataAccessObject<T> where T : class
    {
        Task CreateAsync(T dto, Action<AsyncOptions> setup = null);

        Task UpdateAsync(T dto, Action<AsyncOptions> setup = null);
    }
}
