using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio.Repositories
{
    /// <summary>
    /// Defines a repository complying to the Event Sourcing concept.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the model.</typeparam>
    /// <typeparam name="TEvent">The type of events associated with the model.</typeparam>
    public interface IEventSourcingRepository<TModel, in TKey, in TEvent> : IRepository<TModel> where TModel : class, IIdentity<TKey>
    {
        /// <summary>
        /// Reads a stream of events asynchronous that rehydrates the state of a model.
        /// </summary>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="fromVersion">The version from where to start reading.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the rehydrated model.</returns>
        Task<TModel> ReadAsync(TKey id, long fromVersion = 0, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Writes a stream of <paramref name="events"/> asynchronous that dehydrates the state of a model.
        /// </summary>
        /// <param name="id">The identifier of the model.</param>
        /// <param name="events">The events associated with the model to persist.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task WriteAsync(TKey id, IEnumerable<TEvent> events, Action<AsyncOptions> setup = null);
    }
}
