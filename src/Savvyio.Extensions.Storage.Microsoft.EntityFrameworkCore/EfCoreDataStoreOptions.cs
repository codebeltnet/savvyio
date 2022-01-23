using System;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.Storage
{
    /// <summary>
    /// Configuration options for <see cref="IEfCoreDataStore"/>.
    /// </summary>
    public class EfCoreDataStoreOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataStoreOptions"/> class.
        /// </summary>
        public EfCoreDataStoreOptions()
        {
        }

        /// <summary>
        /// Gets or sets the delegate responsible for configuring the model of the <see cref="DbContext"/>.
        /// </summary>
        /// <value>The delegate responsible for configuring the model of the <see cref="DbContext"/>.</value>
        public Action<ModelBuilder> ModelConstructor { get; set; }

        /// <summary>
        /// Gets or sets the delegate responsible for configuring defaults and conventions of the <see cref="DbContext"/>.
        /// </summary>
        /// <value>The delegate responsible for configuring defaults and conventions of the <see cref="DbContext"/>.</value>
        public Action<ModelConfigurationBuilder> ConventionsConfigurator { get; set; }

        /// <summary>
        /// Gets or sets the delegate responsible for configuring the <see cref="DbContext"/>.
        /// </summary>
        /// <value>The delegate responsible for configuring the <see cref="DbContext"/>.</value>
        public Action<DbContextOptionsBuilder> ContextConfigurator { get; set; }
    }

    /// <summary>
    /// Configuration options for <see cref="IEfCoreDataStore{TMarker}"/>.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that these options represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDataStoreOptions" />
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public class EfCoreDataStoreOptions<TMarker> : EfCoreDataStoreOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}
