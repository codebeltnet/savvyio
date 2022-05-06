using System;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        /// Adds an <see cref="EfCoreDataStore" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore(this IServiceCollection services, Action<EfCoreDataStoreOptions> setup)
        {
            services.AddDataStore<IEfCoreDataStore, EfCoreDataStore>();
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataStore{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore<TMarker>(this IServiceCollection services, Action<EfCoreDataStoreOptions<TMarker>> setup)
        {
            services.AddDataStore<IEfCoreDataStore<TMarker>, EfCoreDataStore<TMarker>, TMarker>();
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="IEfCoreDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the configured implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IEfCoreDataStore"/>
        /// <seealso cref="IEfCoreDataStore{TMarker}"/>
        /// <seealso cref="IUnitOfWork"/>
        /// <seealso cref="IUnitOfWork{TMarker}"/>
        public static IServiceCollection AddEfCoreDataStore<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IEfCoreDataStore
        {
            Validator.ThrowIfNull(services, nameof(services));
            var efCoreDataStoreType = typeof(IEfCoreDataStore);
            var unitOfWorkType = typeof(IUnitOfWork);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                efCoreDataStoreType = typeof(IEfCoreDataStore<>).MakeGenericType(markerType);
                unitOfWorkType = typeof(IUnitOfWork<>).MakeGenericType(markerType);
            }
            services.TryAddScoped(efCoreDataStoreType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(unitOfWorkType, p => p.GetRequiredService<TImplementation>());
            return services;
        }

        /// <summary>
        /// Adds an <see cref="EfCoreRepository{TEntity,TKey}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreRepository{TEntity,TKey}"/> will be type forwarded to: <see cref="IWritableRepository{TEntity,TKey}" />, <see cref="IReadableRepository{TEntity,TKey}" />, <see cref="ISearchableRepository{TEntity,TKey}" /> and <see cref="IDeletableRepository{TEntity,TKey}" />.</remarks>
        /// <seealso cref="EfCoreRepository{TEntity,TKey}"/>
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey>(this IServiceCollection services)
            where TEntity : class, IIdentity<TKey>
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
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey, TMarker>(this IServiceCollection services)
            where TEntity : class, IIdentity<TKey>
        {
            return services.AddRepository<EfCoreRepository<TEntity, TKey, TMarker>, TEntity, TKey>();
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataAccessObject{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <seealso cref="EfCoreDataAccessObject{T}"/>
        public static IServiceCollection AddEfCoreDataAccessObject<T>(this IServiceCollection services)
            where T : class
        {
            return services.AddDataAccessObject<EfCoreDataAccessObject<T>, T>();
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataAccessObject{T,TMarker}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data access object represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <seealso cref="EfCoreDataAccessObject{T,TMarker}"/>
        public static IServiceCollection AddEfCoreDataAccessObject<T, TMarker>(this IServiceCollection services)
            where T : class
        {
            return services.AddDataAccessObject<EfCoreDataAccessObject<T, TMarker>, T>();
        }
    }
}
