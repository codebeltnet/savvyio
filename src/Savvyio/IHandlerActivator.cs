using System.Threading;
using System.Threading.Tasks;

namespace Savvyio
{
    /// <summary>
    /// Specifies a way of invoking delegates that handles <typeparamref name="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to invoke.</typeparam>
    /// <seealso cref="IHandlerRegistry{TModel}"/>
    public interface IHandlerActivator<in TModel>
    {
        /// <summary>
        /// Invokes (if registered) the delegate handling the specified <paramref name="model"/>.
        /// </summary>
        /// <param name="model">The model that is handled by a delegate.</param>
        /// <returns><c>true</c> if the delegate handling the specified <paramref name="model"/> was invoked, <c>false</c> otherwise.</returns>
        bool TryInvoke(TModel model);

        /// <summary>
        /// Invokes (if registered) the function delegate handling the specified <paramref name="model"/>.
        /// </summary>
        /// <param name="model">The model that is handled by a function delegate.</param>
        /// <param name="ct">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><c>true</c> if the function delegate handling the specified <paramref name="model"/> was invoked, <c>false</c> otherwise.</returns>
        Task<bool> TryInvokeAsync(TModel model, CancellationToken ct = default);
    }
}
