using System;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddEfCoreDataSource_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(AnotherDbMarker)));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataSource>(sut2.GetRequiredService<IEfCoreDataSource>());
            Assert.IsType<EfCoreDataSource>(sut2.GetRequiredService<IDataSource>());
            Assert.IsType<EfCoreDataSource>(sut2.GetRequiredService<IUnitOfWork>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataSource>(), sut2.GetRequiredService<IUnitOfWork>());
        }

        [Fact]
        public void AddEfCoreDataSource_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataSource<DbMarker>>(sut2.GetRequiredService<IEfCoreDataSource<DbMarker>>());
            Assert.IsType<EfCoreDataSource<DbMarker>>(sut2.GetRequiredService<IDataSource<DbMarker>>());
            Assert.IsType<EfCoreDataSource<DbMarker>>(sut2.GetRequiredService<IUnitOfWork<DbMarker>>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataSource<DbMarker>>(), sut2.GetRequiredService<IUnitOfWork<DbMarker>>());
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddEfCoreRepository<Account, long>();
            sut1.AddEfCoreRepository<PlatformProvider, Guid>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreRepository<Account, long>>(sut2.GetRequiredService<IPersistentRepository<Account, long>>());
            Assert.IsType<EfCoreRepository<PlatformProvider, Guid>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid>>());
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(DbMarker)));
            sut1.AddEfCoreRepository<Account, long, DbMarker>();
            sut1.AddEfCoreRepository<PlatformProvider, Guid, DbMarker>();
            sut1.AddEfCoreDataSource<AnotherDbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(AnotherDbMarker)));
            sut1.AddEfCoreRepository<Account, long, AnotherDbMarker>();
            sut1.AddEfCoreRepository<PlatformProvider, Guid, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IPersistentRepository<Account, long, DbMarker>>());
            Assert.IsType<EfCoreRepository<PlatformProvider, Guid, DbMarker>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, DbMarker>>());
            Assert.IsType<EfCoreRepository<Account, long, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentRepository<Account, long, AnotherDbMarker>>());
            Assert.IsType<EfCoreRepository<PlatformProvider, Guid, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentRepository<Account, long, DbMarker>>(), sut2.GetRequiredService<IPersistentRepository<Account, long, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, DbMarker>>(), sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, AnotherDbMarker>>());
        }
        
        [Fact]
        public void AddEfCoreDataAccessObject_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddDefaultEfCoreDataStore<Account>();
            sut1.AddDefaultEfCoreDataStore<PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DefaultEfCoreDataStore<Account>>(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>>>());
            Assert.IsType<DefaultEfCoreDataStore<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>>>());
        }

        [Fact]
        public void AddEfCoreDataAccessObject_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(DbMarker)));
            sut1.AddDefaultEfCoreDataStore<Account, DbMarker>();
            sut1.AddDefaultEfCoreDataStore<PlatformProvider, DbMarker>();
            sut1.AddEfCoreDataSource<AnotherDbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(AnotherDbMarker)));
            sut1.AddDefaultEfCoreDataStore<Account, AnotherDbMarker>();
            sut1.AddDefaultEfCoreDataStore<PlatformProvider, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DefaultEfCoreDataStore<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, DbMarker>>());
            Assert.IsType<DefaultEfCoreDataStore<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, DbMarker>>());
            Assert.IsType<DefaultEfCoreDataStore<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, AnotherDbMarker>>());
            Assert.IsType<DefaultEfCoreDataStore<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, AnotherDbMarker>>());
        }
    }
}
