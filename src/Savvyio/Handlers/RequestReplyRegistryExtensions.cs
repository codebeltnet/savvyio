using System;
using System.Threading.Tasks;

namespace Savvyio.Handlers
{
    /// <summary>
    /// Extension methods for the <see cref="IRequestReplyRegistry{TRequest}"/> interface.
    /// </summary>
    public static class RequestReplyRegistryExtensions
    {
        /// <summary>
        /// Registers the asynchronous.
        /// </summary>
        /// <typeparam name="TRequest">The type of the model to store in the registry.</typeparam>
        /// <typeparam name="TResponse">The type of the response to return.</typeparam>
        /// <param name="registry">The <see cref="IRequestReplyRegistry{TRequest}"/> to extend.</param>
        /// <param name="handler">The function delegate that handles <typeparamref name="TRequest" />.</param>
        public static void RegisterAsync<TRequest, TResponse>(this IRequestReplyRegistry<TRequest> registry, Func<TRequest, Task<TResponse>> handler) where TRequest : class
        {
            registry.RegisterAsync<TRequest, TResponse>((h, _) => handler(h));
        }
    }
}
