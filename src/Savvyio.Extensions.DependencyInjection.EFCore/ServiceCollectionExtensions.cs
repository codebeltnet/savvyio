using System;
using Cuemon;
using Cuemon.Extensions;
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
        /// <param name="serviceSetup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreDataSource(this IServiceCollection services, Action<EfCoreDataSourceOptions> dataSourceSetup, Action<EfCoreServiceOptions> serviceSetup = null)
        {
            services.AddEfCoreDataSource<EfCoreDataSource>(serviceSetup);
            return services.Configure(dataSourceSetup);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataSource{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="EfCoreDataSourceOptions{TMarker}" /> which need to be configured.</param>
        /// <param name="serviceSetup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreDataSource<TMarker>(this IServiceCollection services, Action<EfCoreDataSourceOptions<TMarker>> dataSourceSetup, Action<EfCoreServiceOptions> serviceSetup = null)
        {
            services.AddEfCoreDataSource<EfCoreDataSource<TMarker>>(serviceSetup);
            return services.Configure(dataSourceSetup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="IEfCoreDataSource" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IEfCoreDataSource"/> to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IEfCoreDataSource"/>
        /// <seealso cref="IEfCoreDataSource{TMarker}"/>
        /// <seealso cref="IUnitOfWork"/>
        /// <seealso cref="IUnitOfWork{TMarker}"/>
        public static IServiceCollection AddEfCoreDataSource<TService>(this IServiceCollection services, Action<EfCoreServiceOptions> setup = null) where TService : class, IEfCoreDataSource
        {
            Validator.ThrowIfNull(services, nameof(services));
            var options = setup.Configure();
            var unitOfWorkType = typeof(IUnitOfWork);
            if (typeof(TService).TryGetDependencyInjectionMarker(out var markerType))
            {
                unitOfWorkType = typeof(IUnitOfWork<>).MakeGenericType(markerType);
            }
            return services.AddDataSource<TService>(Patterns.ConfigureExchange<EfCoreServiceOptions, ServiceOptions>(setup)).TryAdd(unitOfWorkType, p => p.GetRequiredService<TService>(), options.Lifetime);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreRepository{TEntity,TKey}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreRepository{TEntity,TKey}"/> will be type forwarded to: <see cref="IWritableRepository{TEntity,TKey}" />, <see cref="IReadableRepository{TEntity,TKey}" />, <see cref="ISearchableRepository{TEntity,TKey}" /> and <see cref="IDeletableRepository{TEntity,TKey}" />.</remarks>
        /// <seealso cref="EfCoreRepository{TEntity,TKey}"/>
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey>(this IServiceCollection services) where TEntity : class, IIdentity<TKey>
        {
            return services.AddRepository<EfCoreRepository<TEntity, TKey>, TEntity, TKey>();
        }

        /// <summary>
        /// Adds an <see cref="EfCoreRepository{TEntity,TKey,TMarker}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreRepository{TEntity,TKey,TMarker}"/> will be type forwarded to: <see cref="IWritableRepository{TEntity,TKey,TMarker}" />, <see cref="IReadableRepository{TEntity,TKey,TMarker}" />, <see cref="ISearchableRepository{TEntity,TKey,TMarker}" /> and <see cref="IDeletableRepository{TEntity,TKey,TMarker}" />.</remarks>
        /// <seealso cref="EfCoreRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey, TMarker>(this IServiceCollection services) where TEntity : class, IIdentity<TKey>
        {
            return services.AddRepository<EfCoreRepository<TEntity, TKey, TMarker>, TEntity, TKey>();
        }

        /// <summary>
        /// Adds an implementation of <see cref="EfCoreDataStore{T,TOptions}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="EfCoreDataStore{T,TOptions}"/> to add.</typeparam>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore<TService, T>(this IServiceCollection services)
            where TService : EfCoreDataStore<T, EfCoreQueryOptions<T>>
            where T : class
        {
            return services.AddDataStore<TService, T, EfCoreQueryOptions<T>>();
        }

        /// <summary>
        /// Adds a <see cref="DefaultEfCoreDataStore{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDefaultEfCoreDataStore<T>(this IServiceCollection services) where T : class
        {
            return services.AddEfCoreDataStore<DefaultEfCoreDataStore<T>, T>();
        }

        /// <summary>
        /// Adds a <see cref="DefaultEfCoreDataStore{T,TMarker}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDefaultEfCoreDataStore<T, TMarker>(this IServiceCollection services) where T : class
        {
            return services.AddEfCoreDataStore<DefaultEfCoreDataStore<T, TMarker>, T>();
        }
    }
}
