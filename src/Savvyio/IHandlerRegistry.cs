using System;
using System.Threading;
using System.Threading.Tasks;

namespace Savvyio
{
    /// <summary>
    /// Specifies a registry that store delegates responsible of handling type <typeparamref name="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to store in the registry.</typeparam>
    public interface IHandlerRegistry<in TModel>
    {
        /// <summary>
        /// Registers the specified delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The object that implements <typeparamref name="TModel"/>.</typeparam>
        /// <param name="handler">The delegate that handles <typeparamref name="T"/>.</param>
        void Register<T>(Action<T> handler) where T : class, TModel;

        /// <summary>
        /// Registers the specified function delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The object that implements <typeparamref name="TModel"/>.</typeparam>
        /// <param name="handler">The function delegate that handles <typeparamref name="T"/>.</param>
        void RegisterAsync<T>(Func<T, Task> handler) where T : class, TModel;

        /// <summary>
        /// Registers the specified function delegate <paramref name="handler"/>.
        /// </summary>
        /// <typeparam name="T">The object that implements <typeparamref name="TModel"/>.</typeparam>
        /// <param name="handler">The function delegate that handles <typeparamref name="T"/>.</param>
        void RegisterAsync<T>(Func<T, CancellationToken, Task> handler) where T : class, TModel;
    }
}
