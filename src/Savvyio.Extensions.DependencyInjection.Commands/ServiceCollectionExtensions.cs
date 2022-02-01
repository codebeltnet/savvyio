using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Commands;

namespace Savvyio.Extensions.DependencyInjection.Commands
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default implementation of the <see cref="ICommandDispatcher"/> interface.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services)
        {
            services.TryAddScoped<ICommandDispatcher, CommandDispatcher>();
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="ICommandHandler" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddCommandHandler<TImplementation>(this IServiceCollection services) where TImplementation : class, ICommandHandler
        {
            services.TryAddTransient<ICommandHandler, TImplementation>();
            return services;
        }
    }
}
