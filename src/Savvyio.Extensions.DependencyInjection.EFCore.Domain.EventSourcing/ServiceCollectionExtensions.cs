using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.Domain.EventSourcing;
using Savvyio.Extensions.EFCore.Domain;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="EfCoreAggregateRepository{TEntity,TKey}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> will be type forwarded accordingly.</remarks>
        /// <seealso cref="EfCoreAggregateRepository{TEntity,TKey}"/>
        public static IServiceCollection AddEfCoreTracedAggregateRepository<TEntity, TKey>(this IServiceCollection services)
            where TEntity : class, ITracedAggregateRoot<TKey>
        {
            return services.AddTracedAggregateRepository<EfCoreTracedAggregateRepository<TEntity, TKey>, TEntity, TKey>();
        }

        /// <summary>
        /// Adds an <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>The <see cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/> will be type forwarded accordingly.</remarks>
        /// <seealso cref="EfCoreAggregateRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddEfCoreTracedAggregateRepository<TEntity, TKey, TMarker>(this IServiceCollection services)
            where TEntity : class, ITracedAggregateRoot<TKey>
        {
            return services.AddTracedAggregateRepository<EfCoreTracedAggregateRepository<TEntity, TKey, TMarker>, TEntity, TKey>();
        }
    }
}
