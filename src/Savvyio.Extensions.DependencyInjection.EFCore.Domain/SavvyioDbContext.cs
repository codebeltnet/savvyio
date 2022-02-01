using Cuemon.Configuration;
using Cuemon.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
{
    internal class SavvyioDbContext<TMarker> : DbContext, IConfigurable<EfCoreDataStoreOptions>, IDependencyInjectionMarker<TMarker>
    {
        public SavvyioDbContext(IOptions<EfCoreDataStoreOptions<TMarker>> options)
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
