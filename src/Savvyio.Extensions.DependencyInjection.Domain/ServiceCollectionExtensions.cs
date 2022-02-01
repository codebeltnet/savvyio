using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Storage;

namespace Savvyio.Extensions.DependencyInjection.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IAggregateRepository{TEntity,TKey}" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IAggregateRepository{TEntity,TKey}"/>
        /// <seealso cref="IAggregateRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="IPersistentRepository{TEntity,TKey}"/>
        /// <seealso cref="IPersistentRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddAggregateRepository<TImplementation, TEntity, TKey>(this IServiceCollection services)
            where TImplementation : class, IAggregateRepository<TEntity, TKey>
            where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
        {
            Validator.ThrowIfNull(services, nameof(services));
            var entityType = typeof(TEntity);
            var keyType = typeof(TKey);
            var aggregateRepositoryType = typeof(IAggregateRepository<,>).MakeGenericType(entityType, keyType);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                aggregateRepositoryType = typeof(IAggregateRepository<,,>).MakeGenericType(entityType, keyType, markerType);
            }
            services.TryAddScoped(aggregateRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.AddRepository<TImplementation, TEntity, TKey>();
            return services;
        }

        /// <summary>
        /// Adds a default implementation of the <see cref="IDomainEventDispatcher"/> interface.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services)
        {
            services.TryAddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDomainEventHandler" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddDomainEventHandler<TImplementation>(this IServiceCollection services) where TImplementation : class, IDomainEventHandler
        {
            services.TryAddTransient<IDomainEventHandler, TImplementation>();
            return services;
        }
    }
}
