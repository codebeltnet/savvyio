using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Storage
{
    /// <summary>
    /// Defines a generic way of abstracting readable repositories (cRud).
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IRepository{TEntity,TKey}"/>
    public interface IReadableRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        /// <summary>
        /// Loads the entity from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the entity.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the entity of the operation or <c>null</c> if not found.</returns>
        Task<TEntity> GetByIdAsync(TKey id, Action<AsyncOptions> setup = null);
    }
}
