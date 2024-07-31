using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DapperExtensions;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.DependencyInjection.DapperExtensions
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="DapperExtensionsDataStore{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDapperExtensionsDataStore<T>(this IServiceCollection services) where T : class
        {
            return services.AddDataStore<DapperExtensionsDataStore<T>, T, DapperExtensionsQueryOptions<T>>();
        }

        /// <summary>
        /// Adds an implementation of <see cref="DapperExtensionsDataStore{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO to use.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for DapperExtensions.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddDapperExtensionsDataStore<T, TMarker>(this IServiceCollection services) where T : class
        {
            return services.AddDataStore<DapperExtensionsDataStore<T, TMarker>,  T, DapperExtensionsQueryOptions<T>>();
        }
    }
}
