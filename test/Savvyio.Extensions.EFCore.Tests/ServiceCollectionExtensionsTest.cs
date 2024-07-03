using System;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Data;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.EFCore
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
            sut1.AddDataSource<EfCoreDataSource>()
                .AddUnitOfWork<EfCoreDataSource>();
            sut1.AddConfiguredOptions<EfCoreDataSourceOptions>(_ => {});
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataSource>(sut2.GetRequiredService<IEfCoreDataSource>());
            Assert.IsType<EfCoreDataSource>(sut2.GetRequiredService<IUnitOfWork>());
            Assert.Same(sut2.GetRequiredService<IEfCoreDataSource>(), sut2.GetRequiredService<IUnitOfWork>());
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions>>().Value.ContextConfigurator);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions>>().Value.ModelConstructor);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions>>().Value.ConventionsConfigurator);
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddDefaultImplementation()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount();
                o.ConventionsConfigurator = _ => { };
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataSource>(sut2.GetRequiredService<IEfCoreDataSource>());
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions>>().Value.ContextConfigurator);
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions>>().Value.ModelConstructor);
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions>>().Value.ConventionsConfigurator);
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddDefaultImplementationWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource<Account>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount();
            });
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataSource<Account>>(sut2.GetRequiredService<IEfCoreDataSource<Account>>());
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions<Account>>>().Value.ContextConfigurator);
            Assert.NotNull(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions<Account>>>().Value.ModelConstructor);
            Assert.Null(sut2.GetRequiredService<IOptions<EfCoreDataSourceOptions<Account>>>().Value.ConventionsConfigurator);
        }

        [Fact]
        public void AddEfCoreRepository_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o =>
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
            sut1.AddEfCoreDataSource<DbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddEfCoreRepository<Account, long, DbMarker>();
            sut1.AddEfCoreRepository<PlatformProvider, Guid, DbMarker>();
            sut1.AddEfCoreDataSource<AnotherDbMarker>(o =>
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
        public void AddEfCoreDataStore_ShouldAddManyImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddEfCoreDataStore<Account>();
            sut1.AddEfCoreDataStore<PlatformProvider>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore<Account>>(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>>>());
            Assert.IsType<EfCoreDataStore<PlatformProvider>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>>>());
        }

        [Fact]
        public void AddEfCoreDataStore_ShouldAddManyImplementationsWithMarkers()
        {
            var sut1 = new ServiceCollection();
            sut1.AddEfCoreDataSource<DbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddEfCoreDataStore<Account, DbMarker>();
            sut1.AddEfCoreDataStore<PlatformProvider, DbMarker>();
            sut1.AddEfCoreDataSource<AnotherDbMarker>(o =>
            {
                o.ContextConfigurator = builder => builder.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = builder => builder.AddAccount().AddPlatformProvider();
            });
            sut1.AddEfCoreDataStore<Account, AnotherDbMarker>();
            sut1.AddEfCoreDataStore<PlatformProvider, AnotherDbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreDataStore<Account, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, DbMarker>>());
            Assert.IsType<EfCoreDataStore<PlatformProvider, DbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, DbMarker>>());
            Assert.IsType<EfCoreDataStore<Account, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, AnotherDbMarker>>());
            Assert.IsType<EfCoreDataStore<PlatformProvider, AnotherDbMarker>>(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<Account, EfCoreQueryOptions<Account>, AnotherDbMarker>>());
            Assert.NotSame(sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, DbMarker>>(), sut2.GetRequiredService<IPersistentDataStore<PlatformProvider, EfCoreQueryOptions<PlatformProvider>, AnotherDbMarker>>());
        }
    }
}
