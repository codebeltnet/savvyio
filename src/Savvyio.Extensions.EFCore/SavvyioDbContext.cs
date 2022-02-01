using Cuemon.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Savvyio.Extensions.EntityFrameworkCore
{
    internal class SavvyioDbContext : DbContext, IConfigurable<EfCoreDataStoreOptions>
    {
        public SavvyioDbContext(IOptions<EfCoreDataStoreOptions> options)
        {
            Options = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            Options.ContextConfigurator?.Invoke(optionsBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            Options.ConventionsConfigurator?.Invoke(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Options.ModelConstructor?.Invoke(modelBuilder);
        }

        public EfCoreDataStoreOptions Options { get; }
    }
}
