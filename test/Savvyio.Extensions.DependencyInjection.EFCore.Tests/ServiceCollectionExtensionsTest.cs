using System;
using Cuemon.Extensions.Xunit;
using Cuemon.Threading;
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
        public void AddEfCoreDataStore_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(AnotherDbMarker)));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IEfCoreDataStore>());
            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IDataStore>());
            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IUnitOfWork>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataStore>(), sut2.GetRequiredService<IUnitOfWork>());
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore<DbMarker>>(sut2.GetRequiredService<IEfCoreDataStore<DbMarker>>());
            Assert.IsType<EfCoreDataStore<DbMarker>>(sut2.GetRequiredService<IDataStore<DbMarker>>());
            Assert.IsType<EfCoreDataStore<DbMarker>>(sut2.GetRequiredService<IUnitOfWork<DbMarker>>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataStore<DbMarker>>(), sut2.GetRequiredService<IUnitOfWork<DbMarker>>());
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
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
            sut1.AddEfCoreDataStore<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(DbMarker)));
            sut1.AddEfCoreRepository<Account, long, DbMarker>();
            sut1.AddEfCoreRepository<PlatformProvider, Guid, DbMarker>();
            sut1.AddEfCoreDataStore<AnotherDbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(AnotherDbMarker)));
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
            sut1.AddEfCoreDataStore(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddEfCoreDataAccessObject<Account>();
            sut1.AddEfCoreDataAccessObject<PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataAccessObject<Account>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>>>());
            Assert.IsType<EfCoreDataAccessObject<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>>>());
        }

        [Fact]
        public void AddEfCoreDataAccessObject_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(DbMarker)));
            sut1.AddEfCoreDataAccessObject<Account, DbMarker>();
            sut1.AddEfCoreDataAccessObject<PlatformProvider, DbMarker>();
            sut1.AddEfCoreDataStore<AnotherDbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(AnotherDbMarker)));
            sut1.AddEfCoreDataAccessObject<Account, AnotherDbMarker>();
            sut1.AddEfCoreDataAccessObject<PlatformProvider, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataAccessObject<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, DbMarker>>());
            Assert.IsType<EfCoreDataAccessObject<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, DbMarker>>());
            Assert.IsType<EfCoreDataAccessObject<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, AnotherDbMarker>>());
            Assert.IsType<EfCoreDataAccessObject<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, AnotherDbMarker>>());
        }
    }
}
