using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Handlers
{
    /// <summary>
    /// Specifies a Request-Reply/In-Out MEP registry that store delegates responsible of handling type <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to store in the registry.</typeparam>
    /// <seealso cref="IRequestReplyActivator{TModel}"/>
    public interface IRequestReplyRegistry<in TRequest>
    {
        /// <summary>
        /// Registers the specified function delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <typeparamref name="TRequest"/>.</typeparam>
        /// <typeparam name="TResponse">The type of the response to return.</typeparam>
        /// <param name="handler">The function delegate that handles <typeparamref name="T"/>.</param>
        void Register<T, TResponse>(Func<T, TResponse> handler) where T : class, TRequest;

        /// <summary>
        /// Registers the specified function delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <typeparamref name="TRequest"/>.</typeparam>
        /// <typeparam name="TResponse">The type of the response to return.</typeparam>
        /// <param name="handler">The function delegate that handles <typeparamref name="T"/>.</param>
        void RegisterAsync<T, TResponse>(Func<T, CancellationToken, Task<TResponse>> handler) where T : class, TRequest;
    }
}
