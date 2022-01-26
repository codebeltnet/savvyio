using System;
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
        /// Adds an <see cref="EfCoreDataStore" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore(this IServiceCollection services, Action<EfCoreDataStoreOptions> setup)
        {
            AddEfCoreDataStore<EfCoreDataStore>(services);
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
            AddEfCoreDataStore<EfCoreDataStore<TMarker>>(services);
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
        /// Adds an <see cref="EfCoreRepository{TEntity,TKey,TMarker}"/> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>This method supports a convenient way of adding an implementation of a <see cref="EfCoreRepository{TEntity,TKey,TMarker}"/> where <c>TEntity</c> and <c>TMarker</c> is <typeparamref name="TEntity"/>.</remarks>
        /// <seealso cref="EfCoreRepository{TEntity,TKey,TMarker}"/>
        public static IServiceCollection AddEfCoreRepository<TEntity, TKey>(this IServiceCollection services)
            where TEntity : class, IAggregateRoot<IDomainEvent, TKey>
        {
            AddEfCoreRepository<EfCoreRepository<TEntity, TKey, TEntity>, TEntity, TKey>(services);
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
        /// <remarks>The implementation will be type forwarded to: <see cref="IWritableRepository{TEntity,TKey}" />, <see cref="IReadableRepository{TEntity,TKey}" /> and <see cref="IDeletableRepository{TEntity,TKey}" />.</remarks>
        public static IServiceCollection AddEfCoreRepository<TImplementation, TEntity, TKey>(this IServiceCollection services)
            where TImplementation : class, IPersistentRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
        {
            Validator.ThrowIfNull(services, nameof(services));
            var entityType = typeof(TEntity);
            var keyType = typeof(TKey);
            var efPersistentRepositoryType = typeof(IPersistentRepository<,>);
            var efWritableRepositoryType = typeof(IWritableRepository<,>);
            var efReadableRepositoryType = typeof(IReadableRepository<,>);
            var efDeletableRepositoryType = typeof(IDeletableRepository<,>);
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
        /// Adds an <see cref="EfCoreDataAccessObject{T,TMarker}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>This method supports a convenient way of adding an implementation of a <see cref="EfCoreDataAccessObject{T,TMarker}"/> where <c>T</c> and <c>TMarker</c> is <typeparamref name="T"/>.</remarks>
        /// <seealso cref="EfCoreDataAccessObject{T,TMarker}"/>
        public static IServiceCollection AddEfCoreDataAccessObject<T>(this IServiceCollection services)
            where T : class
        {
            AddEfCoreDataAccessObject<EfCoreDataAccessObject<T, T>, T>(services);
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
        public static IServiceCollection AddEfCoreDataAccessObject<TImplementation, T>(this IServiceCollection services)
            where TImplementation : class, IPersistentDataAccessObject<T> where T : class
        {
            Validator.ThrowIfNull(services, nameof(services));
            var dtoType = typeof(T);
            var efDataAccessObjectType = typeof(IPersistentDataAccessObject<>);
            var efWritableDataAccessObjectType = typeof(IWritableDataAccessObject<>);
            var efReadableDataAccessObjectType = typeof(IReadableDataAccessObject<>);
            var efDeletableDataAccessObjectType = typeof(IDeletableDataAccessObject<>);
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
