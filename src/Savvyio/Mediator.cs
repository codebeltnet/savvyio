using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;

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

        private void Dispatch<TModel, THandler>(TModel model, Func<THandler, IHandlerActivator<TModel>> handlerFactory, string parameterName) where THandler : IHandler<TModel>
        {
            Validator.ThrowIfNull(model, parameterName);
            var handlerType = typeof(THandler);
            var modelType = model.GetType();
            var hasHandler = false;
            if (_serviceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    hasHandler |= handlerFactory(handler).TryInvoke(model);
                }
            }
            if (!hasHandler) { throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TModel).Name}: {modelType.FullName}.", parameterName); }
        }

        private async Task DispatchAsync<TModel, THandler>(TModel model, Func<THandler, IHandlerActivator<TModel>> handlerFactory, Action<AsyncOptions> setup, string parameterName) where THandler : IHandler<TModel>
        {
            Validator.ThrowIfNull(model, parameterName);
            var options = setup.Configure();
            var handlerType = typeof(THandler);
            var modelType = model.GetType();
            var hasHandler = false;
            if (_serviceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    hasHandler |= await handlerFactory(handler).TryInvokeAsync(model, options.CancellationToken).ConfigureAwait(false);
                }
            }
            if (!hasHandler) { throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TModel).Name}: {modelType.FullName}.", parameterName); }
        }
    }
}
