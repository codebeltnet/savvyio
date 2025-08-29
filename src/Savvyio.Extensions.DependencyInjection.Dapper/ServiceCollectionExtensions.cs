using System;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DependencyInjection.Data;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="DapperDataSource" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="DapperDataSourceOptions" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddDapperDataSource(this IServiceCollection services, Action<DapperDataSourceOptions> dataSourceSetup, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                .AddDapperDataSource<DapperDataSource>(serviceSetup)
                .AddConfiguredOptions(dataSourceSetup);
        }

        /// <summary>
        /// Adds an <see cref="DapperDataSource{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="DapperDataSourceOptions{TMarker}" /> that needs to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddDapperDataSource<TMarker>(this IServiceCollection services, Action<DapperDataSourceOptions<TMarker>> dataSourceSetup, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                .AddDapperDataSource<DapperDataSource<TMarker>>(serviceSetup)
                .AddConfiguredOptions(dataSourceSetup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDapperDataSource" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IDapperDataSource"/> interface to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDapperDataSource"/>
        /// <seealso cref="IDapperDataSource{TMarker}"/>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddDapperDataSource<TService>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TService : class, IDapperDataSource
        {
            Validator.ThrowIfNull(services);
            return services.AddDataSource<TService>(setup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="DapperDataStore{T,TOptions}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="DapperDataStore{T,TOptions}"/> abstraction to add.</typeparam>
        /// <typeparam name="T">The type of the DTO to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddDapperDataStore<TService, T>(this IServiceCollection services)
            where TService : DapperDataStore<T, DapperQueryOptions>
            where T : class
        {
            Validator.ThrowIfNull(services);
            return services.AddDataStore<TService, T, DapperQueryOptions>();
        }
    }
}
