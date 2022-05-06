using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
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
        public static IServiceCollection AddRepository<TImplementation, TEntity, TKey>(this IServiceCollection services)
            where TImplementation : class, IPersistentRepository<TEntity, TKey>
            where TEntity : class, IIdentity<TKey>
        {
            Validator.ThrowIfNull(services, nameof(services));
            var entityType = typeof(TEntity);
            var keyType = typeof(TKey);
            var persistentRepositoryType = typeof(IPersistentRepository<,>).MakeGenericType(entityType, keyType);
            var writableRepositoryType = typeof(IWritableRepository<,>).MakeGenericType(entityType, keyType);
            var readableRepositoryType = typeof(IReadableRepository<,>).MakeGenericType(entityType, keyType);
            var searchableRepositoryType = typeof(ISearchableRepository<,>).MakeGenericType(entityType, keyType);
            var deletableRepositoryType = typeof(IDeletableRepository<,>).MakeGenericType(entityType, keyType);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                persistentRepositoryType = typeof(IPersistentRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                writableRepositoryType = typeof(IWritableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                searchableRepositoryType = typeof(ISearchableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                readableRepositoryType = typeof(IReadableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                deletableRepositoryType = typeof(IDeletableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
            }
            services.TryAddScoped(persistentRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(writableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(readableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(searchableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(deletableRepositoryType, p => p.GetRequiredService<TImplementation>());
            return services;
        }
    }
}
