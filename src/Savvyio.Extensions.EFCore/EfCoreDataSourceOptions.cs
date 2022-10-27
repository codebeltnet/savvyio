using System;
using Cuemon.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Configuration options for <see cref="IEfCoreDataSource"/>.
    /// </summary>
    public class EfCoreDataSourceOptions : IParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataSourceOptions"/> class.
        /// </summary>
        public EfCoreDataSourceOptions()
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
