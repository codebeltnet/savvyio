using System;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Domain;

namespace Savvyio.Extensions.DependencyInjection.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IPersistentRepository{TEntity,TKey}" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IPersistentRepository{TEntity,TKey}"/> to add.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IPersistentRepository{TEntity,TKey}"/>
        /// <seealso cref="IPersistentRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="IWritableRepository{TEntity,TKey}"/>
        /// <seealso cref="IWritableRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="IReadableRepository{TEntity,TKey}"/>
        /// <seealso cref="IReadableRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="ISearchableRepository{TEntity,TKey}"/>
        /// <seealso cref="ISearchableRepository{TEntity,TKey,TMarker}"/>
        /// <seealso cref="IDeletableRepository{TEntity,TKey}"/>
        /// <seealso cref="IDeletableRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddRepository<TService, TEntity, TKey>(this IServiceCollection services, Action<ServiceOptions> setup = null)
            where TService : class, IPersistentRepository<TEntity, TKey>
            where TEntity : class, IIdentity<TKey>
        {
            Validator.ThrowIfNull(services);
            var options = Patterns.Configure(setup ?? (o => o.Lifetime = ServiceLifetime.Scoped));
            return services.Add<TService>(o =>
            {
                o.Lifetime = options.Lifetime;
                o.NestedTypePredicate = type => type.HasInterfaces(typeof(IRepository<,>));
            });
        }

        /// <summary>
        /// Adds an implementation of <see cref="IUnitOfWork" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IUnitOfWork"/> to add.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IUnitOfWork"/>
        /// <seealso cref="IUnitOfWork{TMarker}"/>
        public static IServiceCollection AddUnitOfWork<TService>(this IServiceCollection services, Action<ServiceOptions> setup = null) where TService : class, IUnitOfWork
        {
            Validator.ThrowIfNull(services);
            var options = (setup ?? (o => o.Lifetime = ServiceLifetime.Scoped)).Configure();
            var unitOfWorkType = typeof(IUnitOfWork);
            if (typeof(TService).TryGetDependencyInjectionMarker(out var markerType))
            {
                unitOfWorkType = typeof(IUnitOfWork<>).MakeGenericType(markerType);
            }
            return services.Add(unitOfWorkType, p => p.GetRequiredService<TService>(), options.Lifetime);
        }
    }
}
