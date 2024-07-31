using System.Threading;
using System.Threading.Tasks;
using Cuemon;

namespace Savvyio.Handlers
{
    /// <summary>
    /// Specifies a way of invoking Request-Reply/In-Out MEP delegates that handles <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <seealso cref="IRequestReplyRegistry{TModel}"/>
    public interface IRequestReplyActivator<in TRequest>
    {
        /// <summary>
        /// Invokes (if registered) the delegate handling the specified <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The model that is handled by a delegate.</param>
        /// <param name="result">The value returned from the request; otherwise the default value for <typeparamref name="TResponse"/> of the <paramref name="result"/> parameter.</param>
        /// <returns><c>true</c> if a delegate matching the specified <paramref name="request"/> was invoked; otherwise, <c>false</c>.</returns>
        bool TryInvoke<TResponse>(TRequest request, out TResponse result);

        /// <summary>
        /// Invokes (if registered) the function delegate handling the specified <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="request">The model that is handled by a function delegate.</param>
        /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ConditionalValue"/> that indicates if a function delegate matching the specified <paramref name="request"/> was invoked and with the potential result hereof.</returns>
        Task<ConditionalValue<TResponse>> TryInvokeAsync<TResponse>(TRequest request, CancellationToken ct = default);
    }
}
