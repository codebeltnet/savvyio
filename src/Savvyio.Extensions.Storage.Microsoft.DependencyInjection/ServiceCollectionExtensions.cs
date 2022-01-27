using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Domain;
using Savvyio.Storage;

namespace Savvyio.Extensions.Storage
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
        /// <seealso cref="IDeletableRepository{TEntity,TKey}"/>
        /// <seealso cref="IDeletableRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddRepository<TImplementation, TEntity, TKey>(this IServiceCollection services)
            where TImplementation : class, IPersistentRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>, IAggregateRoot<IDomainEvent, TKey>
        {
            Validator.ThrowIfNull(services, nameof(services));
            var entityType = typeof(TEntity);
            var keyType = typeof(TKey);
            var efPersistentRepositoryType = typeof(IPersistentRepository<,>).MakeGenericType(entityType, keyType);
            var efWritableRepositoryType = typeof(IWritableRepository<,>).MakeGenericType(entityType, keyType);
            var efReadableRepositoryType = typeof(IReadableRepository<,>).MakeGenericType(entityType, keyType);
            var efDeletableRepositoryType = typeof(IDeletableRepository<,>).MakeGenericType(entityType, keyType);
            services.TryAddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                efPersistentRepositoryType = typeof(IPersistentRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                efWritableRepositoryType = typeof(IWritableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                efReadableRepositoryType = typeof(IReadableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                efDeletableRepositoryType = typeof(IDeletableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
            }
            services.TryAddScoped(efPersistentRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(efWritableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(efReadableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.TryAddScoped(efDeletableRepositoryType, p => p.GetRequiredService<TImplementation>());
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
            var efDataAccessObjectType = typeof(IPersistentDataAccessObject<>).MakeGenericType(dtoType);
            var efWritableDataAccessObjectType = typeof(IWritableDataAccessObject<>).MakeGenericType(dtoType);
            var efReadableDataAccessObjectType = typeof(IReadableDataAccessObject<>).MakeGenericType(dtoType);
            var efDeletableDataAccessObjectType = typeof(IDeletableDataAccessObject<>).MakeGenericType(dtoType);
            services.TryAddTransient<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                efDataAccessObjectType = typeof(IPersistentDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                efWritableDataAccessObjectType = typeof(IWritableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                efReadableDataAccessObjectType = typeof(IReadableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                efDeletableDataAccessObjectType = typeof(IDeletableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
            }
            services.TryAddTransient(efDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(efWritableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(efReadableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(efDeletableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            return services;
        }
    }
}
