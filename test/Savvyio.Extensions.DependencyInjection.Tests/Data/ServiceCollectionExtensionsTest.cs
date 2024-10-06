using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Assets;
using Savvyio.Extensions.DependencyInjection.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.Data
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
            sut1.AddDataSource<FakeDataSource>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataSource>(sut2.GetRequiredService<IDataSource>());
        }

        [Fact]
        public void AddDataStore_ShouldAddDefaultImplementation_Implicit()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataSource>(sut2.GetRequiredService<IDataSource>());
        }

        [Fact]
        public void AddDataStore_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource<DbMarker>>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataSource<DbMarker>>(sut2.GetRequiredService<IDataSource<DbMarker>>());
        }

        [Fact]
        public void AddDataStore_ShouldAddDefaultImplementationWithMarker_Implicit()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource<DbMarker>>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataSource<DbMarker>>(sut2.GetRequiredService<IDataSource<DbMarker>>());
        }

        [Fact]
        public void AddRepository_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource>();
            sut1.AddRepository<FakeRepository<Account, long>, Account, long>();
            sut1.AddRepository<FakeRepository<PlatformProvider, Guid>, PlatformProvider, Guid>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeRepository<Account, long>>(sut2.GetRequiredService<IPersistentRepository<Account, long>>());
            Assert.IsType<FakeRepository<PlatformProvider, Guid>>(sut2.GetRequiredService<IPersistentRepository<PlatformProvider, Guid>>());
        }

        [Fact]
        public void AddRepository_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource<DbMarker>>();
            sut1.AddRepository<FakeRepository<Account, long, DbMarker>, Account, long>();
            sut1.AddRepository<FakeRepository<PlatformProvider, Guid, DbMarker>, PlatformProvider, Guid>();
            sut1.AddDataSource<FakeDataSource<AnotherDbMarker>>();
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
        public void AddDataAccessObject_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource>();
            sut1.AddDataStore<FakeDataStore<Account>, Account>();
            sut1.AddDataStore<FakeDataStore<PlatformProvider>, PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataStore<Account>>(sut2.GetRequiredService<IPersistentDataStore<Account, AsyncOptions>>());
            Assert.IsType<FakeDataStore<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, AsyncOptions>>());
        }

        [Fact]
        public void AddDataAccessObject_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddDataSource<FakeDataSource<DbMarker>>();
            sut1.AddDataStore<FakeDataStore<Account, DbMarker>, Account>();
            sut1.AddDataStore<FakeDataStore<PlatformProvider, DbMarker>, PlatformProvider>();
            sut1.AddDataSource<FakeDataSource<AnotherDbMarker>>();
            sut1.AddDataStore<FakeDataStore<Account, AnotherDbMarker>, Account>();
            sut1.AddDataStore<FakeDataStore<PlatformProvider, AnotherDbMarker>, PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<FakeDataStore<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<Account, AsyncOptions, DbMarker>>());
            Assert.IsType<FakeDataStore<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, AsyncOptions, DbMarker>>());
            Assert.IsType<FakeDataStore<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<Account, AsyncOptions, AnotherDbMarker>>());
            Assert.IsType<FakeDataStore<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, AsyncOptions, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<Account, AsyncOptions, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<Account, AsyncOptions, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, AsyncOptions, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, AsyncOptions, AnotherDbMarker>>());
        }
    }
}
