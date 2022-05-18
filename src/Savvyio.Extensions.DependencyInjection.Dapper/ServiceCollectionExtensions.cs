using System;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        /// Adds an <see cref="DapperDataStore" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="DapperDataStoreOptions" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddDapperDataStore(this IServiceCollection services, Action<DapperDataStoreOptions> setup)
        {
            services.AddDapperDataStore<DapperDataStore>();
            return services.Configure(setup);
        }

        /// <summary>
        /// Adds an <see cref="DapperDataStore{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="DapperDataStoreOptions{TMarker}" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddDapperDataStore<TMarker>(this IServiceCollection services, Action<DapperDataStoreOptions<TMarker>> setup)
        {
            services.AddDapperDataStore<DapperDataStore<TMarker>>();
            return services.Configure(setup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDapperDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the configured implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDapperDataStore"/>
        /// <seealso cref="IDapperDataStore{TMarker}"/>
        public static IServiceCollection AddDapperDataStore<TImplementation>(this IServiceCollection services) where TImplementation : class, IDapperDataStore
        {
            Validator.ThrowIfNull(services, nameof(services));
            var dapperDataStoreType = typeof(IDapperDataStore);
            var dataStoreType = typeof(IDataStore);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                dapperDataStoreType = typeof(IDapperDataStore<>).MakeGenericType(markerType);
                dataStoreType = typeof(IDataStore<>).MakeGenericType(markerType);
            }
            services.TryAddScoped(dapperDataStoreType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(dataStoreType, p => p.GetRequiredService<TImplementation>());
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="DapperDataAccessObject{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDapperDataAccessObject<TImplementation, T>(this IServiceCollection services) 
            where TImplementation : DapperDataAccessObject<T>
            where T : class
        {
            return services.AddDataAccessObject<TImplementation, T, DapperOptions>();
        }

        /// <summary>
        /// Adds an <see cref="DefaultDapperDataAccessObject{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDefaultDapperDataAccessObject<T>(this IServiceCollection services) where T : class
        {
            return services.AddDapperDataAccessObject<DefaultDapperDataAccessObject<T>, T>();
        }

        /// <summary>
        /// Adds an <see cref="DefaultDapperDataAccessObject{T,TMarker}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDefaultDapperDataAccessObject<T, TMarker>(this IServiceCollection services) where T : class
        {
            return services.AddDapperDataAccessObject<DefaultDapperDataAccessObject<T, TMarker>, T>();
        }
    }
}
