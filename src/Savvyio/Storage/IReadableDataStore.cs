using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting the actual I/O communication with a data store that is responsible of reading data (cRud).
    /// </summary>
    /// <seealso cref="IDataStore" />
    public interface IReadableDataStore : IDataStore
    {
        /// <summary>
        /// Query the data store asynchronous from the specified <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">The type of the Entity/Projection/DTO you want to query.</typeparam>
        /// <param name="predicate">The predicate that filters the query.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result contains the outcome of the query operation.</returns>
        Task<IQueryable<T>> ReadAsync<T>(Expression<Func<T, bool>> predicate, Action<AsyncOptions> setup = null) where T : class;
    }
}
