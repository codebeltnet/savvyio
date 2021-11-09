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
        /// Persist the <paramref name="aggregate"/> Saves the asynchronous.
        /// </summary>
        /// <param name="aggregate">The aggregate.</param>
        /// <param name="setup">The setup.</param>
        /// <returns>Task.</returns>
        Task SaveAsync(TAggregate aggregate, Action<AsyncOptions> setup = null);
    }
}
