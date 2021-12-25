using System;
using System.Threading.Tasks;

namespace Savvyio
{
    /// <summary>
    /// Extension methods for the <see cref="IFireForgetRegistry{TRequest}"/> interface.
    /// </summary>
    public static class FireForgetRegistryExtensions
    {
        /// <summary>
        /// Registers the specified function delegate <paramref name="handler" />.
        /// </summary>
        /// <typeparam name="TRequest">The type of the model to store in the registry.</typeparam>
        /// <param name="registry">The <see cref="IFireForgetRegistry{TRequest}"/> to extend.</param>
        /// <param name="handler">The function delegate that handles <typeparamref name="TRequest" />.</param>
        public static void RegisterAsync<TRequest>(this IFireForgetRegistry<TRequest> registry, Func<TRequest, Task> handler) where TRequest : class
        {
            registry.RegisterAsync<TRequest>((h, _) => handler(h));
        }
    }
}
