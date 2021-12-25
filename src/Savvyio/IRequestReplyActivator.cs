using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;

namespace Savvyio
{
    /// <summary>
    /// Specifies a way of invoking Request-Reply/In-Out MEP delegates that handles <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke.</typeparam>
    /// <seealso cref="IRequestReplyRegistry{TModel}"/>
    public interface IRequestReplyActivator<in TRequest>
    {
        /// <summary>
        /// Invokes (if registered) the delegate handling the specified <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="model">The model that is handled by a delegate.</param>
        /// <param name="result">The value returned from the request; otherwise the default value for <typeparamref name="TResponse"/> of the <paramref name="result"/> parameter.</param>
        /// <returns><c>true</c> if a delegate matching the specified <paramref name="model"/> was invoked; otherwise, <c>false</c>.</returns>
        bool TryInvoke<TResponse>(TRequest model, out TResponse result);

        /// <summary>
        /// Invokes (if registered) the function delegate handling the specified <paramref name="model"/>.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response.</typeparam>
        /// <param name="model">The model that is handled by a function delegate.</param>
        /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ConditionalValue{TResult}"/> that indicates if a function delegate matching the specified <paramref name="model"/> was invoked and with the potential result hereof.</returns>
        Task<ConditionalValue<TResponse>> TryInvokeAsync<TResponse>(TRequest model, CancellationToken ct = default);
    }
}
