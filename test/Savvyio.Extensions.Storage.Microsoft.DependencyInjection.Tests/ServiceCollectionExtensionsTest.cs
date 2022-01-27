using System;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Extensions.Storage.Assets;
using Savvyio.Storage;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Storage
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddDataStore_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataStore<IDataStore, FakeDataStore>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataStore>(sut2.GetRequiredService<IDataStore>());
            Assert.IsType<FakeDataStore>(sut2.GetRequiredService<IUnitOfWork>());
            Assert.Same(sut2.GetRequiredService<IDataStore>(), sut2.GetRequiredService<IUnitOfWork>());
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataStore<FakeDataStore<DbMarker>>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataStore<DbMarker>>(sut2.GetRequiredService<IDataStore<DbMarker>>());
            Assert.IsType<FakeDataStore<DbMarker>>(sut2.GetRequiredService<IUnitOfWork<DbMarker>>());
            Assert.Same(sut2.GetRequiredService<IDataStore<DbMarker>>(), sut2.GetRequiredService<IUnitOfWork<DbMarker>>());
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataStore<FakeDataStore>();
            sut1.AddRepository<FakeRepository<Account, long>, Account, long>();
            sut1.AddRepository<FakeRepository<PlatformProvider, Guid>, PlatformProvider, Guid>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeRepository<Account, long>>(sut2.GetRequiredService<IPersistentRepository<Account, long>>());
            Assert.IsType<FakeRepository<PlatformProvider, Guid>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid>>());
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataStore<FakeDataStore<DbMarker>>();
            sut1.AddRepository<FakeRepository<Account, long, DbMarker>, Account, long>();
            sut1.AddRepository<FakeRepository<PlatformProvider, Guid, DbMarker>, PlatformProvider, Guid>();
            sut1.AddDataStore<FakeDataStore<AnotherDbMarker>>();
            sut1.AddRepository<FakeRepository<Account, long, AnotherDbMarker>, Account, long>();
            sut1.AddRepository<FakeRepository<PlatformProvider, Guid, AnotherDbMarker>, PlatformProvider, Guid>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeRepository<Account, long, DbMarker>>(sut2.GetRequiredService<IPersistentRepository<Account, long, DbMarker>>());
            Assert.IsType<FakeRepository<PlatformProvider, Guid, DbMarker>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, DbMarker>>());
            Assert.IsType<FakeRepository<Account, long, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentRepository<Account, long, AnotherDbMarker>>());
            Assert.IsType<FakeRepository<PlatformProvider, Guid, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentRepository<Account, long, DbMarker>>(), sut2.GetRequiredService<IPersistentRepository<Account, long, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, DbMarker>>(), sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid, AnotherDbMarker>>());
        }
        
        [Fact]
        public void AddEfCoreDataAccessObject_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataStore<FakeDataStore>();
            sut1.AddDataAccessObject<FakeDataAccessObject<Account>, Account>();
            sut1.AddDataAccessObject<FakeDataAccessObject<PlatformProvider>, PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataAccessObject<Account>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account>>());
            Assert.IsType<FakeDataAccessObject<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider>>());
        }

        [Fact]
        public void AddEfCoreDataAccessObject_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataStore<FakeDataStore<DbMarker>>();
            sut1.AddDataAccessObject<FakeDataAccessObject<Account, DbMarker>, Account>();
            sut1.AddDataAccessObject<FakeDataAccessObject<PlatformProvider, DbMarker>, PlatformProvider>();
            sut1.AddDataStore<FakeDataStore<AnotherDbMarker>>();
            sut1.AddDataAccessObject<FakeDataAccessObject<Account, AnotherDbMarker>, Account>();
            sut1.AddDataAccessObject<FakeDataAccessObject<PlatformProvider, AnotherDbMarker>, PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataAccessObject<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, DbMarker>>());
            Assert.IsType<FakeDataAccessObject<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DbMarker>>());
            Assert.IsType<FakeDataAccessObject<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, AnotherDbMarker>>());
            Assert.IsType<FakeDataAccessObject<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<Account, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<Account, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, AnotherDbMarker>>());
        }
    }
}
