using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Savvyio.Handlers;

namespace Savvyio.Dispatchers
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

        /// <summary>
        /// Dispatches the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <typeparam name="TRequest">The type of the input model registered to a handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler to locate the input model.</typeparam>
        /// <param name="request">The model that is being handled by a registered delegate.</param>
        /// <param name="handlerFactory">The function delegate that will locate and invoke the handler registered to the specified <paramref name="request"/>.</param>
        /// <remarks>A <paramref name="request"/> can be a command, domain event, integration event, query etc.</remarks>
        protected virtual void Dispatch<TRequest, THandler>(TRequest request, Func<THandler, IFireForgetActivator<TRequest>> handlerFactory) 
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(handlerFactory, nameof(handlerFactory));
            var handlerType = typeof(THandler);
            var hasHandler = false;
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers) // allow multiple handlers for same model
                {
                    hasHandler |= handlerFactory(handler).TryInvoke(request);
                }
            }
            if (!hasHandler) { throw OrphanedHandlerException.Create<TRequest, THandler>(request, nameof(request)); }
        }

        /// <summary>
        /// Dispatches the specified <paramref name="request"/> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <typeparam name="TRequest">The type of the input model registered to a handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler to locate the input model.</typeparam>
        /// <param name="request">The model that is being handled by a registered delegate.</param>
        /// <param name="handlerFactory">The function delegate that will locate and invoke the handler registered to the specified <paramref name="request"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>A <paramref name="request"/> can be a command, domain event, integration event, query etc.</remarks>
        protected virtual async Task DispatchAsync<TRequest, THandler>(TRequest request, Func<THandler, IFireForgetActivator<TRequest>> handlerFactory, Action<AsyncOptions> setup) 
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(handlerFactory, nameof(handlerFactory));
            var options = setup.Configure();
            var handlerType = typeof(THandler);
            var hasHandler = false;
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers) // allow multiple handlers for same model
                {
                    var operation = await handlerFactory(handler).TryInvokeAsync(request, options.CancellationToken).ConfigureAwait(false);
                    hasHandler |= operation.Succeeded;
                }
            }
            if (!hasHandler) { throw OrphanedHandlerException.Create<TRequest, THandler>(request, nameof(request)); }
        }
    }
}
