using Cuemon.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.Extensions.DapperExtensions;
using Savvyio.Extensions.DependencyInjection.Dapper;
using Savvyio.Extensions.DependencyInjection.Data;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.DapperExtensions
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDapperExtensionsDataStore_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            sut1.AddDapperExtensionsDataStore<AccountProjection>();
            sut1.AddDapperExtensionsDataStore<PlatformProviderProjection>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DapperExtensionsDataStore<AccountProjection>>(sut2.GetRequiredService<IPersistentDataStore<AccountProjection, DapperExtensionsQueryOptions<AccountProjection>>>());
            Assert.IsType<DapperExtensionsDataStore<PlatformProviderProjection>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProviderProjection, DapperExtensionsQueryOptions<PlatformProviderProjection>>>());
        }

        [Fact]
        public void AddDapperExtensionsDataStore_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataSource<DbMarker>(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            sut1.AddDapperExtensionsDataStore<AccountProjection, DbMarker>();
            sut1.AddDapperExtensionsDataStore<PlatformProviderProjection, DbMarker>();
            sut1.AddDapperDataSource<AnotherDbMarker>(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            sut1.AddDapperExtensionsDataStore<AccountProjection, AnotherDbMarker>();
            sut1.AddDapperExtensionsDataStore<PlatformProviderProjection, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DapperExtensionsDataStore<AccountProjection, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<AccountProjection, DapperExtensionsQueryOptions<AccountProjection>, DbMarker>>());
            Assert.IsType<DapperExtensionsDataStore<PlatformProviderProjection, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProviderProjection, DapperExtensionsQueryOptions<PlatformProviderProjection>, DbMarker>>());
            Assert.IsType<DapperExtensionsDataStore<AccountProjection, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<AccountProjection, DapperExtensionsQueryOptions<AccountProjection>, AnotherDbMarker>>());
            Assert.IsType<DapperExtensionsDataStore<PlatformProviderProjection, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProviderProjection, DapperExtensionsQueryOptions<PlatformProviderProjection>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<AccountProjection, DapperExtensionsQueryOptions<AccountProjection>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<AccountProjection, DapperExtensionsQueryOptions<AccountProjection>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<PlatformProviderProjection, DapperExtensionsQueryOptions<PlatformProviderProjection>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<PlatformProviderProjection, DapperExtensionsQueryOptions<PlatformProviderProjection>, AnotherDbMarker>>());
        }
    }
}
