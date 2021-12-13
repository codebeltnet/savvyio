using System;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Savvyio.Queries;

namespace Savvyio
{
    public interface IMediator
    {
        void Commit(ICommand command);

        void Publish(IIntegrationEvent @event);

        void Publish(IDomainEvent @event);

        Task CommitAsync(ICommand command, Action<AsyncOptions> setup = null);

        Task PublishAsync(IIntegrationEvent @event, Action<AsyncOptions> setup = null);

        Task PublishAsync(IDomainEvent @event, Action<AsyncOptions> setup = null);

        TResult Query<TResult>(IQuery<TResult> query);

        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, Action<AsyncOptions> setup = null);
    }
}
