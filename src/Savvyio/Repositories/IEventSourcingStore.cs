using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Repositories
{
    /// <summary>
    /// Defines a store that does the actual I/O communication optimized for Event Sourcing.
    /// </summary>
    /// <seealso cref="IStore" />
    public interface IEventSourcingStore : IStore
    {
        /// <summary>
        /// Reads a stream of events from a data store asynchronous.
        /// </summary>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
        /// <typeparam name="TEvent">The type of events associated with the model.</typeparam>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="fromVersion">The version from where to start reading.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a sequence of events.</returns>
        Task<IEnumerable<TEvent>> ReadStreamAsync<TKey, TEvent>(TKey id, long fromVersion = 0, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Writes a stream of <paramref name="events"/> to the data store asynchronous.
        /// </summary>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
        /// <typeparam name="TEvent">The type of events associated with the model.</typeparam>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="events">The events to write to the data store.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task WriteStreamAsync<TKey, TEvent>(TKey id, IEnumerable<TEvent> events, Action<AsyncOptions> setup = null);
    }
}
