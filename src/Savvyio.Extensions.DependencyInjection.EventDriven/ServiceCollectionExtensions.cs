using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.EventDriven;

namespace Savvyio.Extensions.DependencyInjection.EventDriven
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default implementation of the <see cref="IIntegrationEventDispatcher"/> interface.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddIntegrationEventDispatcher(this IServiceCollection services)
        {
            services.TryAddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="IIntegrationEventHandler" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddIntegrationEventHandler<TImplementation>(this IServiceCollection services) where TImplementation : class, IIntegrationEventHandler
        {
            services.TryAddTransient<IIntegrationEventHandler, TImplementation>();
            return services;
        }
    }
}
