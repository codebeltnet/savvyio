using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Storage;

namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Defines a store that does the actual I/O communication optimized for Event Sourcing.
    /// </summary>
    /// <typeparam name="TContext">The type of the context that this event sourcing store represents.</typeparam>
    /// <seealso cref="IDataStore" />
    public interface IEventSourcingDataStore : IDataStore
    {
        /// <summary>
        /// Creates a new entity in the data store asynchronous.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the entity to persist.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="entity">The entity to persist.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the newly created entity.</returns>
        Task<TAggregate> CreateStreamAsync<TAggregate, TKey>(TAggregate entity, Action<AsyncOptions> setup = null) where TAggregate : class, ITracedAggregateRoot<TKey>;

        /// <summary>
        /// Loads an entity from a data store asynchronous.
        /// </summary>
        /// <typeparam name="TAggregate">The type of the entity to load.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="id">The identifier of the entity.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the loaded entity.</returns>
        Task<TAggregate> ReadStreamAsync<TAggregate, TKey>(TKey id, Action<AsyncOptions> setup = null) where TAggregate : class, ITracedAggregateRoot<TKey>;
    }
}
