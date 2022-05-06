using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain.EventSourcing.Storage
{
    /// <summary>
    /// Defines a repository complying to the Event Sourcing concept.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the model.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
    public interface IEventSourcingRepository<TAggregate, TKey> : IWritableRepository<TAggregate, TKey> where TAggregate : class, ITracedAggregateRoot<TKey>
    {
        /// <summary>
        /// Reads a stream of events asynchronous that rehydrates the state of a model.
        /// </summary>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the rehydrated model.</returns>
        Task<TAggregate> GetByIdAsync(TKey id, Action<AsyncOptions> setup = null);
    }
}
