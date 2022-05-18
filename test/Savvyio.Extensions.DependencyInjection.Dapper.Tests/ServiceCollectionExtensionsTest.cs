using System;
using Cuemon.Extensions.Xunit;
using Cuemon.Threading;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.Dapper
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDapperDataStore_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataStore(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DapperDataStore>(sut2.GetRequiredService<IDapperDataStore>());
            Assert.IsType<DapperDataStore>(sut2.GetRequiredService<IDataStore>());
            Assert.Same(sut2.GetRequiredService<IDapperDataStore>(), sut2.GetRequiredService<IDataStore>());
        }

        [Fact]
        public void AddDapperDataStore_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataStore<DbMarker>(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DapperDataStore<DbMarker>>(sut2.GetRequiredService<IDapperDataStore<DbMarker>>());
            Assert.IsType<DapperDataStore<DbMarker>>(sut2.GetRequiredService<IDataStore<DbMarker>>());
            Assert.Same(sut2.GetRequiredService<IDapperDataStore<DbMarker>>(), sut2.GetRequiredService<IDataStore<DbMarker>>());
        }

        [Fact]
        public void AddDapperDataAccessObject_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataStore(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            sut1.AddDefaultDapperDataAccessObject<Account>();
            sut1.AddDefaultDapperDataAccessObject<PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DefaultDapperDataAccessObject<Account>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, DapperOptions>>());
            Assert.IsType<DefaultDapperDataAccessObject<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DapperOptions>>());
        }

        [Fact]
        public void AddDapperDataAccessObject_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDapperDataStore<DbMarker>(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            sut1.AddDefaultDapperDataAccessObject<Account, DbMarker>();
            sut1.AddDefaultDapperDataAccessObject<PlatformProvider, DbMarker>();
            sut1.AddDapperDataStore<AnotherDbMarker>(o => o.ConnectionFactory = () => new SqliteConnection("Data Source=:memory:"));
            sut1.AddDefaultDapperDataAccessObject<Account, AnotherDbMarker>();
            sut1.AddDefaultDapperDataAccessObject<PlatformProvider, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DefaultDapperDataAccessObject<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, DapperOptions, DbMarker>>());
            Assert.IsType<DefaultDapperDataAccessObject<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DapperOptions, DbMarker>>());
            Assert.IsType<DefaultDapperDataAccessObject<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, DapperOptions, AnotherDbMarker>>());
            Assert.IsType<DefaultDapperDataAccessObject<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DapperOptions, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<Account, DapperOptions, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<Account, DapperOptions, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DapperOptions, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DapperOptions, AnotherDbMarker>>());
        }
    }
}
