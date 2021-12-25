using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Queries;

namespace Savvyio
{
    public interface IMediator
    {
        void Commit(ICommand command);

        Task CommitAsync(ICommand command, Action<AsyncOptions> setup = null);

        /// <summary>
        /// Publishes the specified <paramref name="event"/>.
        /// </summary>
        /// <param name="event">The <see cref="IEvent"/> to publish.</param>
        void Publish(IEvent @event);
        
        /// <summary>
        /// Publishes the specified <paramref name="event"/> asynchronous.
        /// </summary>
        /// <param name="event">The <see cref="IEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task PublishAsync(IEvent @event, Action<AsyncOptions> setup = null);

        TResult Query<TResult>(IQuery<TResult> query);

        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, Action<AsyncOptions> setup = null);
    }
}
