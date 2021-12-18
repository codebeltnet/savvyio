using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Savvyio.Queries;

namespace Savvyio
{
    public class Mediator : IMediator
    {
        private readonly Func<Type, IEnumerable<object>> _serviceFactory;

        public Mediator(Func<Type, IEnumerable<object>> serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public void Commit(ICommand command)
        {
            try
            {
                Dispatch<ICommand, ICommandHandler>(command, handler => handler.Commands, nameof(command));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Publish(IIntegrationEvent @event)
        {
            try
            {
                Dispatch<IIntegrationEvent, IIntegrationEventHandler>(@event, handler => handler.IntegrationEvents, nameof(@event));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Publish(IDomainEvent @event)
        {
            try
            {
                Dispatch<IDomainEvent, IDomainEventHandler>(@event, handler => handler.DomainEvents, nameof(@event));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task CommitAsync(ICommand command, Action<AsyncOptions> setup = null)
        {
            try
            {
                return DispatchAsync<ICommand, ICommandHandler>(command, handler => handler.Commands, setup, nameof(command));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task PublishAsync(IIntegrationEvent @event, Action<AsyncOptions> setup = null)
        {
            try
            {
                return DispatchAsync<IIntegrationEvent, IIntegrationEventHandler>(@event, handler => handler.IntegrationEvents, setup, nameof(@event));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task PublishAsync(IDomainEvent @event, Action<AsyncOptions> setup = null)
        {
            try
            {
                return DispatchAsync<IDomainEvent, IDomainEventHandler>(@event, handler => handler.DomainEvents, setup, nameof(@event));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TResult Query<TResult>(IQuery<TResult> query)
        {
            Validator.ThrowIfNull(query, nameof(query));
            var handlerType = typeof(IQueryHandler);
            var modelType = query.GetType();
            if (_serviceFactory(handlerType) is IEnumerable<IQueryHandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    if (handler.Queries.TryInvoke(query, out TResult result))
                    {
                        return result;
                    }
                }
            }
            throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {modelType.Name}: {modelType.FullName}.", nameof(query));
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query,Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(query, nameof(query));
            var options = setup.Configure();
            var handlerType = typeof(IQueryHandler);
            var modelType = query.GetType();
            if (_serviceFactory(handlerType) is IEnumerable<IQueryHandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    var operation = await handler.Queries.TryInvokeAsync<TResult>(query, options.CancellationToken).ConfigureAwait(false);
                    if (operation.Succeeded)
                    {
                        return operation.Result;
                    }
                }
            }
            throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {nameof(IQuery)}: {modelType.FullName}.", nameof(query));
        }

        private void Dispatch<TModel, THandler>(TModel model, Func<THandler, IFireForgetActivator<TModel>> handlerFactory, string parameterName) where THandler : IHandler<TModel>
        {
            Validator.ThrowIfNull(model, parameterName);
            var handlerType = typeof(THandler);
            var modelType = model.GetType();
            var hasHandler = false;
            if (_serviceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers) // allow multiple handlers for same model
                {
                    hasHandler |= handlerFactory(handler).TryInvoke(model);
                }
            }
            if (!hasHandler) { throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TModel).Name}: {modelType.FullName}.", parameterName); }
        }

        private async Task DispatchAsync<TModel, THandler>(TModel model, Func<THandler, IFireForgetActivator<TModel>> handlerFactory, Action<AsyncOptions> setup, string parameterName) where THandler : IHandler<TModel>
        {
            Validator.ThrowIfNull(model, parameterName);
            var options = setup.Configure();
            var handlerType = typeof(THandler);
            var modelType = model.GetType();
            var hasHandler = false;
            if (_serviceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers) // allow multiple handlers for same model
                {
                    var operation = await handlerFactory(handler).TryInvokeAsync(model, options.CancellationToken).ConfigureAwait(false);
                    hasHandler |= operation.Succeeded;
                }
            }
            if (!hasHandler) { throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TModel).Name}: {modelType.FullName}.", parameterName); }
        }
    }
}
