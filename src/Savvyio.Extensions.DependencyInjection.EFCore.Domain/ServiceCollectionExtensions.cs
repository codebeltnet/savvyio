using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
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
        /// <remarks>The <see cref="EfCoreAggregateDataStore"/> will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreAggregateDataStore(this IServiceCollection services, Action<EfCoreDataStoreOptions> setup)
        {
            services.AddDataStore<IEfCoreDataStore, EfCoreAggregateDataStore>();
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataStore{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateDataStore{TMarker}"/> will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreAggregateDataStore<TMarker>(this IServiceCollection services, Action<EfCoreDataStoreOptions<TMarker>> setup)
        {
            services.AddDataStore<IEfCoreDataStore<TMarker>, EfCoreAggregateDataStore<TMarker>, TMarker>();
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds an <see cref="EfCoreAggregateRepository{TEntity,TKey}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> will be type forwarded accordingly.</remarks>
        /// <seealso cref="EfCoreAggregateRepository{TEntity,TKey}"/>
        public static IServiceCollection AddEfCoreAggregateRepository<TEntity, TKey>(this IServiceCollection services)
            where TEntity : class, IEntity<TKey>, IAggregateRoot<IDomainEvent, TKey>
        {
            return services.AddRepository<EfCoreAggregateRepository<TEntity, TKey>, TEntity, TKey>();
        }

        /// <summary>
        /// Adds an <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> will be type forwarded accordingly.</remarks>
        /// <seealso cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddEfCoreAggregateRepository<TEntity, TKey, TMarker>(this IServiceCollection services)
            where TEntity : class, IEntity<TKey>, IAggregateRoot<IDomainEvent, TKey>
        {
            return services.AddRepository<EfCoreAggregateRepository<TEntity, TKey, TMarker>, TEntity, TKey>();
        }
    }
}
