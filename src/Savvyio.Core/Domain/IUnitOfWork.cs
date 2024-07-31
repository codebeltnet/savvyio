using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines a transaction that bundles multiple <see cref="IRepository{TEntity,TKey}"/> calls into a single unit.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves the different <see cref="IRepository{TEntity,TKey}"/> implementations as one transaction towards a data store asynchronous.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SaveChangesAsync(Action<AsyncOptions> setup = null);
    }
}
