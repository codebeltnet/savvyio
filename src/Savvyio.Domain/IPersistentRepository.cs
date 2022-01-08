using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain
{
    /// <summary>
    /// Defines the persistence part of a Repository.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the Aggregate to persist.</typeparam>
    public interface IPersistentRepository<in TAggregate> where TAggregate : class, IAggregateRoot
    {
        /// <summary>
        /// Persist the <paramref name="aggregate"/> asynchronous.
        /// </summary>
        /// <param name="aggregate">The <see cref="IAggregateRoot"/> to persist.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SaveAsync(TAggregate aggregate, Action<AsyncOptions> setup = null);
    }
}
