using System;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Extensions.DependencyInjection.Domain.EventSourcing
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="ITracedAggregateRepository{TEntity,TKey}" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="ITracedAggregateRepository{TEntity,TKey}"/> interface to add.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="ITracedAggregateRepository{TEntity,TKey}"/>
        /// <seealso cref="ITracedAggregateRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="IWritableRepository{TEntity,TKey}"/>
        /// <seealso cref="IWritableRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="IReadableRepository{TEntity,TKey}"/>
        /// <seealso cref="IReadableRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddTracedAggregateRepository<TService, TEntity, TKey>(this IServiceCollection services, Action<ServiceOptions> setup = null)
            where TService : class, ITracedAggregateRepository<TEntity, TKey>
            where TEntity : class, ITracedAggregateRoot<TKey>
        {
            Validator.ThrowIfNull(services);
            var options = (setup ?? (o => o.Lifetime = ServiceLifetime.Scoped)).Configure();
            return services.Add<TService>(o =>
            {
                o.Lifetime = options.Lifetime;
                o.NestedTypePredicate = type => type.HasInterfaces(typeof(ITracedAggregateRepository<,>), typeof(IWritableRepository<,>), typeof(IReadableRepository<,>));
            });
        }
    }
}
