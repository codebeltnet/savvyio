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
        /// <param name="serviceFactory">The function delegate that provides the services.</param>
        protected RequestReplyDispatcher(Func<Type, IEnumerable<object>> serviceFactory) : base(serviceFactory)
        {
        }

        protected virtual TResult Dispatch<TRequest, THandler, TResult>(TRequest request, Func<THandler, IRequestReplyActivator<TRequest>> handlerFactory) 
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(handlerFactory, nameof(handlerFactory));
            var handlerType = typeof(THandler);
            var requestType = request.GetType();
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    if (handlerFactory(handler).TryInvoke(request, out TResult result))
                    {
                        return result;
                    }
                }
            }
            throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TRequest).Name}: {requestType.FullName}.", nameof(request));
        }

        protected virtual async Task<TResult> DispatchAsync<TRequest, THandler, TResult>(TRequest request, Func<THandler, IRequestReplyActivator<TRequest>> handlerFactory, Action<AsyncOptions> setup)
            where TRequest : IRequest
            where THandler : IHandler<TRequest>
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(handlerFactory, nameof(handlerFactory));
            var options = setup.Configure();
            var handlerType = typeof(THandler);
            var requestType = request.GetType();
            if (ServiceFactory(handlerType) is IEnumerable<THandler> handlers)
            {
                foreach (var handler in handlers)
                {
                    var operation = await handlerFactory(handler).TryInvokeAsync<TResult>(request, options.CancellationToken).ConfigureAwait(false);
                    if (operation.Succeeded)
                    {
                        return operation.Result;
                    }
                }
            }
            throw new OrphanedHandlerException($"Unable to retrieve an {handlerType.Name} for the specified {typeof(TRequest).Name}: {requestType.FullName}.", nameof(request));
        }
    }
}
