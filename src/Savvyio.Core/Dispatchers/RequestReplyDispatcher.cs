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
    /// Provides a generic dispatcher that uses Request-Reply/In-Out MEP. This is an abstract class.
    /// </summary>
    /// <seealso cref="Dispatcher" />
    public abstract class RequestReplyDispatcher : Dispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestReplyDispatcher"/> class.
        /// </summary>
        /// <param name="serviceLocator">The provider of service implementations.</param>
        protected RequestReplyDispatcher(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        /// <summary>
        /// Dispatches the specified <paramref name="request"/> using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TRequest">The type of the input model registered to a handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler to locate the input model.</typeparam>
        /// <typeparam name="TResponse">The type of the output model.</typeparam>
        /// <param name="request">The model that is being handled by a registered delegate.</param>
        /// <param name="handlerFactory">The function delegate that will locate and invoke the handler registered to the specified <paramref name="request"/>.</param>
        /// <returns>The response of the request.</returns>
        /// <remarks>A <paramref name="request"/> can be a command, domain event, integration event, query etc.</remarks>
        protected virtual TResponse Dispatch<TRequest, THandler, TResponse>(TRequest request, Func<THandler, IRequestReplyActivator<TRequest>> handlerFactory)
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request);
            Validator.ThrowIfNull(handlerFactory);
            var handlerType = typeof(THandler);
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    if (handlerFactory(handler).TryInvoke(request, out TResponse result))
                    {
                        return result;
                    }
                }
            }
            throw OrphanedHandlerException.Create<TRequest, THandler>(request, nameof(request));
        }

        /// <summary>
        /// Dispatches the specified <paramref name="request"/> asynchronous using Request-Reply/In-Out MEP.
        /// </summary>
        /// <typeparam name="TRequest">The type of the input model registered to a handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler to locate the input model.</typeparam>
        /// <typeparam name="TResponse">The type of the output model.</typeparam>
        /// <param name="request">The model that is being handled by a registered delegate.</param>
        /// <param name="handlerFactory">The function delegate that will locate and invoke the handler registered to the specified <paramref name="request"/>.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response of the request.</returns>
        /// <remarks>A <paramref name="request"/> can be a command, domain event, integration event, query etc.</remarks>
        protected virtual async Task<TResponse> DispatchAsync<TRequest, THandler, TResponse>(TRequest request, Func<THandler, IRequestReplyActivator<TRequest>> handlerFactory, Action<AsyncOptions> setup)
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request);
            Validator.ThrowIfNull(handlerFactory);
            var options = setup.Configure();
            var handlerType = typeof(THandler);
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    var operation = await handlerFactory(handler).TryInvokeAsync<TResponse>(request, options.CancellationToken).ConfigureAwait(false);
                    if (operation.Succeeded)
                    {
                        return operation.Result;
                    }
                }
            }
            throw OrphanedHandlerException.Create<TRequest, THandler>(request, nameof(request));
        }
    }
}
