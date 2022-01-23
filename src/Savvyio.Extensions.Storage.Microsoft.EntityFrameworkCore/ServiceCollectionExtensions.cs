using System;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Storage;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IEfCoreDataStore" /> to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
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
            services.AddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                efCoreDataStoreType = typeof(IEfCoreDataStore<>).MakeGenericType(markerType);
                unitOfWorkType = typeof(IUnitOfWork<>).MakeGenericType(markerType);
            }
            services.AddScoped(efCoreDataStoreType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(unitOfWorkType, p => p.GetRequiredService<TImplementation>());
            return services;
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataStore" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddEfCoreDataStore(this IServiceCollection services, Action<EfCoreDataStoreOptions> setup)
        {
            Validator.ThrowIfNull(services, nameof(services));
            services.AddScoped<EfCoreDataStore>();
            services.AddScoped<IEfCoreDataStore>(p => p.GetRequiredService<EfCoreDataStore>());
            services.AddScoped<IUnitOfWork>(p => p.GetRequiredService<EfCoreDataStore>());
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
            Validator.ThrowIfNull(services, nameof(services));
            services.AddScoped<EfCoreDataStore<TMarker>>();
            services.AddScoped<IEfCoreDataStore<TMarker>>(p => p.GetRequiredService<EfCoreDataStore<TMarker>>());
            services.AddScoped<IUnitOfWork<TMarker>>(p => p.GetRequiredService<EfCoreDataStore<TMarker>>());
            services.Configure(setup);
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
            services.AddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                efPersistentRepositoryType = typeof(IPersistentRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                efWritableRepositoryType = typeof(IWritableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                efReadableRepositoryType = typeof(IReadableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
                efDeletableRepositoryType = typeof(IDeletableRepository<,,>).MakeGenericType(entityType, keyType, markerType);
            }
            services.AddScoped(efPersistentRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(efWritableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(efReadableRepositoryType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(efDeletableRepositoryType, p => p.GetRequiredService<TImplementation>());
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
            services.AddScoped<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                efDataAccessObjectType = typeof(IPersistentDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                efWritableDataAccessObjectType = typeof(IWritableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                efReadableDataAccessObjectType = typeof(IReadableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
                efDeletableDataAccessObjectType = typeof(IDeletableDataAccessObject<,>).MakeGenericType(dtoType, markerType);
            }
            services.AddScoped(efDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(efWritableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(efReadableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.AddScoped(efDeletableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            return services;
        }
    }
}
