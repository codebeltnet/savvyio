using System;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Defines the whole of a Repository complying to the Event Sourcing concept.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the Aggregate to persist.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this Aggregate.</typeparam>
    public interface IEventSourcingRepository<TAggregate, in TKey> : IPersistentRepository<TAggregate> where TAggregate : class, ITracedAggregateRoot<TKey>
    {
        /// <summary>
        /// Reads a stream of events asynchronous that rehydrates the state of an Aggregate.
        /// </summary>
        /// <param name="id">The identifier of an Aggregate.</param>
        /// <param name="fromVersion">The version from where to start reading.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the rehydrated Aggregate.</returns>
        Task<TAggregate> ReadAsync(TKey id, long fromVersion = 0, Action<AsyncOptions> setup = null);
    }
}
