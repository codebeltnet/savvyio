using Cuemon.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="DbContext"/> class to support Savvy I/O extensions of Microsoft Entity Framework Core.
    /// </summary>
    /// <seealso cref="DbContext" />
    /// <seealso cref="IConfigurable{EfCoreDataStoreOptions}" />
    public class EfCoreDbContext : DbContext, IConfigurable<EfCoreDataSourceOptions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDbContext"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="EfCoreDataSourceOptions" /> which need to be configured.</param>
        public EfCoreDbContext(IOptions<EfCoreDataSourceOptions> setup)
        {
            Options = setup.Value;
        }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        /// <remarks>See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
        /// for more information.</remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            Options.ContextConfigurator?.Invoke(optionsBuilder);
        }

        /// <summary>
        /// Override this method to set defaults and configure conventions before they run. This method is invoked before
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)" />.
        /// </summary>
        /// <param name="configurationBuilder">The builder being used to set defaults and configure conventions that will be used to build the model for this context.</param>
        /// <remarks>If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.</remarks>
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            Options.ConventionsConfigurator?.Invoke(configurationBuilder);
        }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks><para>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information.
        /// </para></remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Options.ModelConstructor?.Invoke(modelBuilder);
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public EfCoreDataSourceOptions Options { get; }
    }
}
