using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="EfCoreDataSource" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="EfCoreDataSourceOptions" /> which need to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreDataSource(this IServiceCollection services, Action<EfCoreDataSourceOptions> dataSourceSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services.AddDataSource<EfCoreDataSource>(serviceSetup)
                .AddUnitOfWork<EfCoreDataSource>(serviceSetup)
                .ConfigureTriple(dataSourceSetup);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataSource{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data source represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="EfCoreDataSourceOptions{TMarker}" /> which need to be configured.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreDataSource<TMarker>(this IServiceCollection services, Action<EfCoreDataSourceOptions<TMarker>> dataSourceSetup, Action<ServiceOptions> serviceSetup = null)
        {
            return services.AddDataSource<EfCoreDataSource<TMarker>>(serviceSetup)
                .AddUnitOfWork<EfCoreDataSource<TMarker>>(serviceSetup)
                .ConfigureTriple(dataSourceSetup);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreRepository{TEntity,TKey}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreRepository{TEntity,TKey}"/> will be type forwarded to: <see cref="IWritableRepository{TEntity,TKey}" />, <see cref="IReadableRepository{TEntity,TKey}" />, <see cref="ISearchableRepository{TEntity,TKey}" /> and <see cref="IDeletableRepository{TEntity,TKey}" />.</remarks>
        /// <seealso cref="EfCoreRepository{TEntity,TKey}"/>
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TEntity : class, IIdentity<TKey>
        {
            return services.AddRepository<EfCoreRepository<TEntity, TKey>, TEntity, TKey>(setup);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreRepository{TEntity,TKey,TMarker}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreRepository{TEntity,TKey,TMarker}"/> will be type forwarded to: <see cref="IWritableRepository{TEntity,TKey,TMarker}" />, <see cref="IReadableRepository{TEntity,TKey,TMarker}" />, <see cref="ISearchableRepository{TEntity,TKey,TMarker}" /> and <see cref="IDeletableRepository{TEntity,TKey,TMarker}" />.</remarks>
        /// <seealso cref="EfCoreRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey, TMarker>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TEntity : class, IIdentity<TKey>
        {
            return services.AddRepository<EfCoreRepository<TEntity, TKey, TMarker>, TEntity, TKey>(setup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="EfCoreDataStore{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore<T>(this IServiceCollection services, Action<ServiceOptions> setup = null) where T : class
        {
            return services.AddDataStore<EfCoreDataStore<T>, T, EfCoreQueryOptions<T>>(setup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="EfCoreDataStore{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore<T, TMarker>(this IServiceCollection services, Action<ServiceOptions> setup = null) where T : class
        {
            return services.AddDataStore<EfCoreDataStore<T, TMarker>, T, EfCoreQueryOptions<T>>(setup);
        }
    }
}
