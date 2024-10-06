using Codebelt.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
{
    public class ServiceCollectionExtensionsTest  : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddEfCoreAggregateRepository_ShouldHaveTypeForwardedImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddEfCoreAggregateRepository<Account, long>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreAggregateRepository<Account, long>>(sut2.GetRequiredService<IAggregateRepository<Account, long>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long>>(sut2.GetRequiredService<IPersistentRepository<Account, long>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long>>(sut2.GetRequiredService<ISearchableRepository<Account, long>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long>>(sut2.GetRequiredService<IDeletableRepository<Account, long>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long>>(sut2.GetRequiredService<IReadableRepository<Account, long>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long>>(sut2.GetRequiredService<IWritableRepository<Account, long>>());
        }

        [Fact]
        public void AddEfCoreAggregateRepository_ShouldHaveTypeForwardedImplementationsWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddEfCoreAggregateRepository<Account, long, DbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreAggregateRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IAggregateRepository<Account, long, DbMarker>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IPersistentRepository<Account, long, DbMarker>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long, DbMarker>>(sut2.GetRequiredService<ISearchableRepository<Account, long, DbMarker>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IDeletableRepository<Account, long, DbMarker>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IReadableRepository<Account, long, DbMarker>>());
            Assert.IsType<EfCoreAggregateRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IWritableRepository<Account, long, DbMarker>>());
        }
    }
}
