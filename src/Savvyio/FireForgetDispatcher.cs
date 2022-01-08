using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;

namespace Savvyio
{
    /// <summary>
    /// Provides a generic dispatcher that uses Fire-and-Forget/In-Only MEP. This is an abstract class.
    /// </summary>
    /// <seealso cref="Dispatcher" />
    public abstract class FireForgetDispatcher : Dispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FireForgetDispatcher"/> class.
        /// </summary>
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        protected FireForgetDispatcher(Func<Type, IEnumerable<object>> serviceFactory) : base(serviceFactory)
        {
        }

        protected virtual void Dispatch<TRequest, THandler>(TRequest request, Func<THandler, IFireForgetActivator<TRequest>> handlerFactory) 
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(handlerFactory, nameof(handlerFactory));
            var handlerType = typeof(THandler);
            var modelType = request.GetType();
            var hasHandler = false;
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers) // allow multiple handlers for same model
                {
                    hasHandler |= handlerFactory(handler).TryInvoke(request);
                }
            }
            if (!hasHandler) { throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TRequest).Name}: {modelType.FullName}.", nameof(request)); }
        }

        protected virtual async Task DispatchAsync<TRequest, THandler>(TRequest request, Func<THandler, IFireForgetActivator<TRequest>> handlerFactory, Action<AsyncOptions> setup) 
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(handlerFactory, nameof(handlerFactory));
            var options = setup.Configure();
            var handlerType = typeof(THandler);
            var modelType = request.GetType();
            var hasHandler = false;
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers) // allow multiple handlers for same model
                {
                    var operation = await handlerFactory(handler).TryInvokeAsync(request, options.CancellationToken).ConfigureAwait(false);
                    hasHandler |= operation.Succeeded;
                }
            }
            if (!hasHandler) { throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TRequest).Name}: {modelType.FullName}.", nameof(request)); }
        }
    }
}
