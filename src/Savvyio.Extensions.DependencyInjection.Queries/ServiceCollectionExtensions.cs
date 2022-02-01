using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Queries;

namespace Savvyio.Extensions.DependencyInjection._Queries
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default implementation of the <see cref="IQueryDispatcher"/> interface.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
        {
            services.TryAddScoped<IQueryDispatcher, QueryDispatcher>();
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="IQueryHandler" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddQueryHandler<TImplementation>(this IServiceCollection services) where TImplementation : class, IQueryHandler
        {
            services.TryAddTransient<IQueryHandler, TImplementation>();
            return services;
        }
    }
}
