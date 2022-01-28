using System;
using Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.Microsoft.EntityFrameworkCore
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
}
