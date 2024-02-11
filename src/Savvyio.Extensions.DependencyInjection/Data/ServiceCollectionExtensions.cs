using System;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Data;

namespace Savvyio.Extensions.DependencyInjection.Data
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an implementation of <see cref="IPersistentDataStore{T,TOptions}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IPersistentDataStore{T,TOptions}"/> to add.</typeparam>
        /// <typeparam name="T">The type of the DTO to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IPersistentDataStore{T,TOptions}"/>
        /// <seealso cref="IPersistentDataStore{T,TOptions,TMarker}"/>
        /// <seealso cref="IWritableDataStore{T}"/>
        /// <seealso cref="IWritableDataStore{T,TMarker}"/>
        /// <seealso cref="IReadableDataStore{T}"/>
        /// <seealso cref="IReadableDataStore{T,TMarker}"/>
        /// <seealso cref="IDeletableDataStore{T}"/>
        /// <seealso cref="IDeletableDataStore{T,TMarker}"/>
        public static IServiceCollection AddDataStore<TService, T>(this IServiceCollection services, Action<ServiceOptions> setup = null)
            where TService : class, IPersistentDataStore<T, AsyncOptions>
            where T : class
        {
            return AddDataStore<TService, T, AsyncOptions>(services, setup);
        }

        /// <summary>
        /// Adds an implementation of <see cref="IPersistentDataStore{T,TOptions}"/> to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the <see cref="IPersistentDataStore{T,TOptions}"/> to add.</typeparam>
        /// <typeparam name="T">The type of the DTO to use.</typeparam>
        /// <typeparam name="TOptions">The type of the options associated with <typeparamref name="T"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="setup">The <see cref="ServiceOptions" /> which may be configured.</param>
        /// <returns>A reference to <paramref name="services" /> so that additional calls can be chained.</returns>
        /// <remarks>If the underlying type of <typeparamref name="TService"/> implements <see cref="IDependencyInjectionMarker{TMarker}"/> interface then this is automatically handled. Also, the implementation will be type forwarded accordingly.</remarks>
        /// <seealso cref="IDependencyInjectionMarker{TMarker}"/>
        /// <seealso cref="IPersistentDataStore{T,TOptions}"/>
        /// <seealso cref="IPersistentDataStore{T,TOptions,TMarker}"/>
        /// <seealso cref="IWritableDataStore{T}"/>
        /// <seealso cref="IWritableDataStore{T,TMarker}"/>
        /// <seealso cref="IReadableDataStore{T}"/>
        /// <seealso cref="IReadableDataStore{T,TMarker}"/>
        /// <seealso cref="IDeletableDataStore{T}"/>
        /// <seealso cref="IDeletableDataStore{T,TMarker}"/>
        public static IServiceCollection AddDataStore<TService, T, TOptions>(this IServiceCollection services, Action<ServiceOptions> setup = null)
            where TService : class, IPersistentDataStore<T, TOptions>
            where T : class
            where TOptions : AsyncOptions, new()
        {
            Validator.ThrowIfNull(services);
            var options = Patterns.Configure(setup);
            return services.Add<TService>(o =>
            {
                o.Lifetime = options.Lifetime;
                o.NestedTypePredicate = type => type.HasInterfaces(typeof(IDataStore<>));
            });
        }
    }
}
