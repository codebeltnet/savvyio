using System.Threading;
using System.Threading.Tasks;
using Cuemon;

namespace Savvyio.Handlers
{
    /// <summary>
    /// Specifies a way of invoking Fire-and-Forget/In-Only MEP delegates that handles <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    /// <seealso cref="IFireForgetRegistry{TModel}"/>
    public interface IFireForgetActivator<in TRequest>
    {
        /// <summary>
        /// Invokes (if registered) the delegate handling the specified <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The model that is handled by a delegate.</param>
        /// <returns><c>true</c> if the delegate handling the specified <paramref name="request"/> was invoked, <c>false</c> otherwise.</returns>
        bool TryInvoke(TRequest request);

        /// <summary>
        /// Invokes (if registered) the function delegate handling the specified <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The model that is handled by a function delegate.</param>
        /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>true</c> if the function delegate handling the specified <paramref name="request"/> was invoked, <c>false</c> otherwise.</returns>
        Task<ConditionalValue> TryInvokeAsync(TRequest request, CancellationToken ct = default);
    }
}
