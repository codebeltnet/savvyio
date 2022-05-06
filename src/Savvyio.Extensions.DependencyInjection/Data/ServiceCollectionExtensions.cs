using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IPersistentDataAccessObject{T,TOptions}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IPersistentDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IPersistentDataAccessObject{T,TOptions,TMarker}"/>
        /// <seealso cref="IWritableDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IWritableDataAccessObject{T,TOptions,TMarker}"/>
        /// <seealso cref="IReadableDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IReadableDataAccessObject{T,TOptions,TMarker}"/>
        /// <seealso cref="IDeletableDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IDeletableDataAccessObject{T,TOptions,TMarker}"/>
        public static IServiceCollection AddDataAccessObject<TImplementation, T>(this IServiceCollection services)
            where TImplementation : class, IPersistentDataAccessObject<T, AsyncOptions>
            where T : class
        {
            return AddDataAccessObject<TImplementation, T, AsyncOptions>(services);
        }

        /// <summary>
        /// Adds an implementation of <see cref="IPersistentDataAccessObject{T,TOptions}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <typeparam name="T">The type of the DTO.</typeparam>
        /// <typeparam name="TOptions">The type of the options associated with this DTO.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TImplementation"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IPersistentDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IPersistentDataAccessObject{T,TOptions,TMarker}"/>
        /// <seealso cref="IWritableDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IWritableDataAccessObject{T,TOptions,TMarker}"/>
        /// <seealso cref="IReadableDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IReadableDataAccessObject{T,TOptions,TMarker}"/>
        /// <seealso cref="IDeletableDataAccessObject{T,TOptions}"/>
        /// <seealso cref="IDeletableDataAccessObject{T,TOptions,TMarker}"/>
        public static IServiceCollection AddDataAccessObject<TImplementation, T, TOptions>(this IServiceCollection services)
            where TImplementation : class, IPersistentDataAccessObject<T, TOptions>
            where T : class
            where TOptions : AsyncOptions, new()
        {
            Validator.ThrowIfNull(services, nameof(services));
            var dtoType = typeof(T);
            var dtoOptionsType = typeof(TOptions);
            var dataAccessObjectType = typeof(IPersistentDataAccessObject<,>).MakeGenericType(dtoType, dtoOptionsType);
            var writableDataAccessObjectType = typeof(IWritableDataAccessObject<,>).MakeGenericType(dtoType, dtoOptionsType);
            var readableDataAccessObjectType = typeof(IReadableDataAccessObject<,>).MakeGenericType(dtoType, dtoOptionsType);
            var deletableDataAccessObjectType = typeof(IDeletableDataAccessObject<,>).MakeGenericType(dtoType, dtoOptionsType);
            services.TryAddTransient<TImplementation>();
            if (typeof(TImplementation).TryGetDependencyInjectionMarker(out var markerType))
            {
                dataAccessObjectType = typeof(IPersistentDataAccessObject<,,>).MakeGenericType(dtoType, dtoOptionsType, markerType);
                writableDataAccessObjectType = typeof(IWritableDataAccessObject<,,>).MakeGenericType(dtoType, dtoOptionsType, markerType);
                readableDataAccessObjectType = typeof(IReadableDataAccessObject<,,>).MakeGenericType(dtoType, dtoOptionsType, markerType);
                deletableDataAccessObjectType = typeof(IDeletableDataAccessObject<,,>).MakeGenericType(dtoType, dtoOptionsType, markerType);
            }
            services.TryAddTransient(dataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(writableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(readableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            services.TryAddTransient(deletableDataAccessObjectType, p => p.GetRequiredService<TImplementation>());
            return services;
        }
    }
}
