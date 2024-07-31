using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio.Handlers
{
    /// <summary>
    /// Specifies a Fire-and-Forget/In-Only MEP registry that store delegates responsible of handling type <typeparamref name="TRequest"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to store in the registry.</typeparam>
    /// <seealso cref="IFireForgetActivator{TModel}"/>
    public interface IFireForgetRegistry<in TRequest>
    {
        /// <summary>
        /// Registers the specified delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <typeparamref name="TRequest"/>.</typeparam>
        /// <param name="handler">The delegate that handles <typeparamref name="T"/>.</param>
        void Register<T>(Action<T> handler) where T : class, TRequest;

        /// <summary>
        /// Registers the specified function delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements <typeparamref name="TRequest"/>.</typeparam>
        /// <param name="handler">The function delegate that handles <typeparamref name="T"/>.</param>
        void RegisterAsync<T>(Func<T, CancellationToken, Task> handler) where T : class, TRequest;
    }
}
