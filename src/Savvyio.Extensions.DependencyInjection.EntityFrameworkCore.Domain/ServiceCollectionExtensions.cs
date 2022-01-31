using System;
using Cuemon;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Extensions.EntityFrameworkCore;
using Savvyio.Extensions.EntityFrameworkCore.Domain;
using Savvyio.Storage;

namespace Savvyio.Extensions.DependencyInjection.EntityFrameworkCore.Domain
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
        public static IServiceCollection AddEfCoreAggregateDataStore(this IServiceCollection services, Action<EfCoreDataStoreOptions> setup)
        {
            services.AddDataStore<IEfCoreDataStore, EfCoreAggregateDataStore>();
            services.Configure(setup);
            return services;
        }

        /// <summary>
        /// Adds an <see cref="EfCoreDataStore{TMarker}" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="setup">The <see cref="EfCoreDataStoreOptions{TMarker}" /> which need to be configured.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional configuration calls can be chained.</returns>
        public static IServiceCollection AddEfCoreAggregateDataStore<TMarker>(this IServiceCollection services, Action<EfCoreDataStoreOptions<TMarker>> setup)
        {
            services.AddDataStore<IEfCoreDataStore<TMarker>, EfCoreAggregateDataStore<TMarker>, TMarker>();
            services.Configure(setup);
            return services;
        }
    }
}
