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

        /// <summary>
        /// Commits the specified <paramref name="request" /> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="ICommand" /> to commit.</param>
        public void Commit(ICommand request)
        {
            _commandDispatcher.Commit(request);
        }

        /// <summary>
        /// Commits the specified <paramref name="request" /> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="ICommand" /> to commit.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public Task CommitAsync(ICommand request, Action<AsyncOptions> setup = null)
        {
            return _commandDispatcher.CommitAsync(request, setup);
        }

        /// <summary>
        /// Raises the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IDomainEvent" /> to raise.</param>
        public void Raise(IDomainEvent request)
        {
            _domainEventDispatcher.Raise(request);
        }

        /// <summary>
        /// Raises the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IDomainEvent" /> to raise.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public Task RaiseAsync(IDomainEvent request, Action<AsyncOptions> setup = null)
        {
            return _domainEventDispatcher.RaiseAsync(request, setup);
        }

        /// <summary>
        /// Publishes the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        public void Publish(IIntegrationEvent request)
        {
            _integrationEventDispatcher.Publish(request);
        }

        /// <summary>
        /// Publishes the specified <paramref name="request"/> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task PublishAsync(IIntegrationEvent request, Action<AsyncOptions> setup = null)
        {
            return _integrationEventDispatcher.PublishAsync(request, setup);
        }

        /// <summary>
        /// Queries the specified <paramref name="request" /> using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to return.</typeparam>
        /// <param name="request">The <see cref="IQuery{TResult}" /> to request.</param>
        /// <returns>The outcome of the query operation.</returns>
        public TResult Query<TResult>(IQuery<TResult> request)
        {
            return _queryDispatcher.Query(request);
        }

        /// <summary>
        /// Queries the specified <paramref name="request"/> asynchronous using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to return.</typeparam>
        /// <param name="request">The <see cref="IQuery{TResult}"/> to request.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the outcome of the query operation.</returns>
        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> request, Action<AsyncOptions> setup = null)
        {
            return _queryDispatcher.QueryAsync(request, setup);
        }
    }
}
