using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.EventDriven;
using Savvyio.Queries;

namespace Savvyio.Extensions
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IMediator"/> interface.
    /// </summary>
    /// <seealso cref="IMediator" />
    public class Mediator : IMediator
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly DomainEventDispatcher _domainEventDispatcher;
        private readonly IntegrationEventDispatcher _integrationEventDispatcher;
        private readonly QueryDispatcher _queryDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mediator"/> class.
        /// </summary>
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        public Mediator(Func<Type, IEnumerable<object>> serviceFactory)
        {
            _commandDispatcher = new CommandDispatcher(serviceFactory);
            _domainEventDispatcher = new DomainEventDispatcher(serviceFactory);
            _integrationEventDispatcher = new IntegrationEventDispatcher(serviceFactory);
            _queryDispatcher = new QueryDispatcher(serviceFactory);
        }

        public void Commit(ICommand request)
        {
            _commandDispatcher.Commit(request);
        }

        public Task CommitAsync(ICommand request, Action<AsyncOptions> setup = null)
        {
            return _commandDispatcher.CommitAsync(request, setup);
        }

        public void Raise(IDomainEvent request)
        {
            _domainEventDispatcher.Raise(request);
        }

        public Task RaiseAsync(IDomainEvent request, Action<AsyncOptions> setup = null)
        {
            return _domainEventDispatcher.RaiseAsync(request, setup);
        }

        public void Publish(IIntegrationEvent request)
        {
            _integrationEventDispatcher.Publish(request);
        }

        public Task PublishAsync(IIntegrationEvent request, Action<AsyncOptions> setup = null)
        {
            return _integrationEventDispatcher.PublishAsync(request, setup);
        }

        public TResult Query<TResult>(IQuery<TResult> request)
        {
            return _queryDispatcher.Query(request);
        }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> request, Action<AsyncOptions> setup = null)
        {
            return _queryDispatcher.QueryAsync(request, setup);
        }
    }
}
