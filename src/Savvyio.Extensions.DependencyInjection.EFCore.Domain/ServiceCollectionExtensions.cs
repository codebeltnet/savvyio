using System;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
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
        /// Adds an <see cref="EfCoreDataSource" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="EfCoreDataSourceOptions" /> which need to be configured.</param>
        /// <param name="serviceSetup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateDataSource"/> will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreAggregateDataSource(this IServiceCollection services, Action<EfCoreDataSourceOptions> dataSourceSetup, Action<EfCoreServiceOptions> serviceSetup = null)
        {
            services.AddEfCoreDataSource<EfCoreAggregateDataSource>(serviceSetup);
            return services.Configure(dataSourceSetup);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataSource{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="dataSourceSetup">The <see cref="EfCoreDataSourceOptions" /> which need to be configured.</param>
        /// <param name="serviceSetup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateDataSource{TMarker}"/> will be type forwarded accordingly.</remarks>
        public static IServiceCollection AddEfCoreAggregateDataSource<TMarker>(this IServiceCollection services, Action<EfCoreDataSourceOptions<TMarker>> dataSourceSetup, Action<EfCoreServiceOptions> serviceSetup = null)
        {
            services.AddEfCoreDataSource<EfCoreAggregateDataSource<TMarker>>(serviceSetup);
            return services.Configure(dataSourceSetup);
        }

        /// <summary>
        /// Adds an <see cref="EfCoreAggregateRepository{TEntity,TKey}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> will be type forwarded accordingly.</remarks>
        /// <seealso cref="EfCoreAggregateRepository{TEntity,TKey}"/>
        public static IServiceCollection AddEfCoreAggregateRepository<TEntity, TKey>(this IServiceCollection services, Action<EfCoreServiceOptions> setup = null) where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
        {
            return services.AddAggregateRepository<EfCoreAggregateRepository<TEntity, TKey>, TEntity, TKey>(Patterns.ConfigureExchange<EfCoreServiceOptions, ServiceOptions>(setup));
        }

        /// <summary>
        /// Adds an <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> will be type forwarded accordingly.</remarks>
        /// <seealso cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddEfCoreAggregateRepository<TEntity, TKey, TMarker>(this IServiceCollection services, Action<EfCoreServiceOptions> setup = null) where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
        {
            return services.AddAggregateRepository<EfCoreAggregateRepository<TEntity, TKey, TMarker>, TEntity, TKey>(Patterns.ConfigureExchange<EfCoreServiceOptions, ServiceOptions>(setup));
        }
    }
}
