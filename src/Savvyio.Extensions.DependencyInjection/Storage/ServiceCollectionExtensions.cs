using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Storage;

namespace Savvyio.Extensions.DependencyInjection.Storage
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TDataStore">The type of the <see cref="IDataStore"/> interface to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDataStore"/>
        /// <seealso cref="IUnitOfWork"/>
        public static IServiceCollection AddDataStore<TDataStore, TImplementation>(this IServiceCollection services)
            where TDataStore : IDataStore
            where TImplementation : class, TDataStore
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.TryAddScoped<TImplementation>();
            services.TryAddScoped(typeof(TDataStore), p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(typeof(IUnitOfWork), p => p.GetRequiredService<TImplementation>());
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TDataStore">The type of the <see cref="IDataStore"/> interface to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the configured implementation to use.</typeparam>
        /// <typeparam name="TMarker">The type used to mark the implementation that this data store represents. Optimized for Microsoft Dependency Injection.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDataStore{TMarker}"/>
        /// <seealso cref="IUnitOfWork{TMarker}"/>
        public static IServiceCollection AddDataStore<TDataStore, TImplementation, TMarker>(this IServiceCollection services)
            where TDataStore : IDataStore<TMarker>
            where TImplementation : class, TDataStore
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.TryAddScoped<TImplementation>();
            services.TryAddScoped(typeof(TDataStore), p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(typeof(IUnitOfWork<TMarker>), p => p.GetRequiredService<TImplementation>());
            return services;
        }

        /// <summary>
        /// Adds an implementation of <see cref="IDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the configured implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IDataStore"/>
        /// <seealso cref="IDataStore{TMarker}"/>
        /// <seealso cref="IUnitOfWork"/>
        /// <seealso cref="IUnitOfWork{TMarker}"/>
        public static IServiceCollection AddDataStore<TImplementation>(this IServiceCollection services)
            where TImplementation : class, IDataStore
        {
            Validator.ThrowIfNull(services, nameof(services));
            var dataStoreType = typeof(IDataStore);
            var unitOfWorkType = typeof(IUnitOfWork);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                dataStoreType = typeof(IDataStore<>).MakeGenericType(markerType);
                unitOfWorkType = typeof(IUnitOfWork<>).MakeGenericType(markerType);
            }
            services.TryAddScoped(dataStoreType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(unitOfWorkType, p => p.GetRequiredService<TImplementation>());
            return services;
        }

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

        /// <summary>
        /// Adds an implementation of <see cref="IPersistentDataAccessObject{T}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IPersistentDataAccessObject{T}"/>
        /// <seealso cref="IPersistentDataAccessObject{T,TMarker}"/>
        /// <seealso cref="IWritableDataAccessObject{T}"/>
        /// <seealso cref="IWritableDataAccessObject{T,TMarker}"/>
        /// <seealso cref="IReadableDataAccessObject{T}"/>
        /// <seealso cref="IReadableDataAccessObject{T,TMarker}"/>
        /// <seealso cref="IDeletableDataAccessObject{T}"/>
        /// <seealso cref="IDeletableDataAccessObject{T,TMarker}"/>
        public static IServiceCollection AddDataAccessObject<TImplementation, T>(this IServiceCollection services)
            where TImplementation : class, IPersistentDataAccessObject<T>
            where T : class
        {
            Validator.ThrowIfNull(services, nameof(services));
            var dtoType = typeof(T);
            var dataAccessObjectType = typeof(IPersistentDataAccessObject<>).MakeGenericType(dtoType);
            var writableDataAccessObjectType = typeof(IWritableDataAccessObject<>).MakeGenericType(dtoType);
            var readableDataAccessObjectType = typeof(IReadableDataAccessObject<>).MakeGenericType(dtoType);
            var deletableDataAccessObjectType = typeof(IDeletableDataAccessObject<>).MakeGenericType(dtoType);
            services.TryAddTransient<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                dataAccessObjectType = typeof(IPersistentDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                writableDataAccessObjectType = typeof(IWritableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                readableDataAccessObjectType = typeof(IReadableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                deletableDataAccessObjectType = typeof(IDeletableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
            }
            services.TryAddTransient(dataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(writableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(readableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(deletableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            return services;
        }
    }
}
