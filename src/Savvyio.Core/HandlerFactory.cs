using System;
using Cuemon;
using Savvyio.Handlers;

namespace Savvyio
{
    /// <summary>
    /// Provides access to factory methods for creating and configuring generic handlers that supports MEP.
    /// </summary>
    public static class HandlerFactory
    {
        /// <summary>
        /// Creates an instance of a Fire-and-Forget/In-Only MEP activator responsible of invoking delegates that handles <typeparamref name="TRequest"/>.
        /// </summary>
        /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
        /// <param name="handlerRegistrar">The delegate responsible for registering delegates of type <typeparamref name="TRequest"/>.</param>
        /// <returns>An implementation of the <see cref="IFireForgetActivator{TRequest}"/> interface responsible of invoking delegates that handles <typeparamref name="TRequest"/>.</returns>
        public static IFireForgetActivator<TRequest> CreateFireForget<TRequest>(Action<IFireForgetRegistry<TRequest>> handlerRegistrar)
        {
            Validator.ThrowIfNull(handlerRegistrar);
            var handlerManager = new FireForgetManager<TRequest>();
            handlerRegistrar(handlerManager);
            return handlerManager;
        }

        /// <summary>
        /// Creates an instance of a Request-Reply/In-Out MEP activator responsible of invoking delegates that handles <typeparamref name="TRequest"/>.
        /// </summary>
        /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
        /// <param name="handlerRegistrar">The delegate responsible for registering delegates of type <typeparamref name="TRequest"/>.</param>
        /// <returns>An implementation of the <see cref="IRequestReplyActivator{TRequest}"/> interface responsible of invoking delegates that handles <typeparamref name="TRequest"/>.</returns>
        public static IRequestReplyActivator<TRequest> CreateRequestReply<TRequest>(Action<IRequestReplyRegistry<TRequest>> handlerRegistrar)
        {
            Validator.ThrowIfNull(handlerRegistrar);
            var handlerManager = new RequestReplyManager<TRequest>();
            handlerRegistrar(handlerManager);
            return handlerManager;
        }
    }
}
