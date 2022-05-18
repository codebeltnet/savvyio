using System;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.EFCore;
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
        public void AddEfCoreDataStore_ShouldAddDefaultImplementationWithDefaultOptions()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore<EfCoreDataStore>();
            sut1.Configure<EfCoreDataStoreOptions>(_ => {});
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IEfCoreDataStore>());
            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IUnitOfWork>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataStore>(), sut2.GetRequiredService<IUnitOfWork>());
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ContextConfigurator);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ModelConstructor);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ConventionsConfigurator);
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount();
                o.ConventionsConfigurator = _ => { };
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore>(sut2.GetRequiredService<IEfCoreDataStore>());
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ContextConfigurator);
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ModelConstructor);
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions>>().Value.ConventionsConfigurator);
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore<Account>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount();
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore<Account>>(sut2.GetRequiredService<IEfCoreDataStore<Account>>());
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions<Account>>>().Value.ContextConfigurator);
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions<Account>>>().Value.ModelConstructor);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataStoreOptions<Account>>>().Value.ConventionsConfigurator);
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
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
            sut1.AddEfCoreDataStore<DbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddEfCoreRepository<Account, long, DbMarker>();
            sut1.AddEfCoreRepository<PlatformProvider, Guid, DbMarker>();
            sut1.AddEfCoreDataStore<AnotherDbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("AnotherDummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
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
            sut1.AddEfCoreDataStore(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddDefaultEfCoreDataAccessObject<Account>();
            sut1.AddDefaultEfCoreDataAccessObject<PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DefaultEfCoreDataAccessObject<Account>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>>>());
            Assert.IsType<DefaultEfCoreDataAccessObject<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>>>());
        }

        [Fact]
        public void AddEfCoreDataAccessObject_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataStore<DbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddDefaultEfCoreDataAccessObject<Account, DbMarker>();
            sut1.AddDefaultEfCoreDataAccessObject<PlatformProvider, DbMarker>();
            sut1.AddEfCoreDataStore<AnotherDbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddDefaultEfCoreDataAccessObject<Account, AnotherDbMarker>();
            sut1.AddDefaultEfCoreDataAccessObject<PlatformProvider, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<DefaultEfCoreDataAccessObject<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, DbMarker>>());
            Assert.IsType<DefaultEfCoreDataAccessObject<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, DbMarker>>());
            Assert.IsType<DefaultEfCoreDataAccessObject<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, AnotherDbMarker>>());
            Assert.IsType<DefaultEfCoreDataAccessObject<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<Account, EfCoreOptions<Account>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataAccessObject<PlatformProvider, EfCoreOptions<PlatformProvider>, AnotherDbMarker>>());
        }
    }
}
