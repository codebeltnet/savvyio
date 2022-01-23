﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Domain.Handlers;
using Savvyio.Assets.Storage;
using Savvyio.Domain;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Storage
{
    public class EfCoreDataStoreTest : Test
    {
        public EfCoreDataStoreTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldFailWithInvalidOperationException_NoDatabaseProvider()
        {
            var sut = new EfCoreDataStore(null, Options.Create(new EfCoreDataStoreOptions()));
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.SaveChangesAsync());
            Assert.StartsWith("No database provider has been configured for this DbContext.", ex.Message);
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldFailWithObjectDisposedException()
        {
            var sut = new EfCoreDataStore(null, o => o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy"));
            sut.Dispose();

            await Assert.ThrowsAsync<ObjectDisposedException>(() => sut.SaveChangesAsync());
            Assert.True(sut.Disposed);
        }

        [Fact]
        public async Task EfCoreDataStore_ShouldRaiseDomainEvents()
        {
            var sut1 = new ServiceCollection();
            sut1.AddScoped<Func<Type, IEnumerable<object>>>(p => p.GetServices);
            sut1.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            sut1.AddScoped<IDomainEventHandler, AccountDomainEventHandler>();
            sut1.AddScoped<ITestStore<IDomainEvent>, DomainEventStore>();
            var sut2 = sut1.BuildServiceProvider();
            var sut3 = new EfCoreDataStore(sut2.GetRequiredService<IDomainEventDispatcher>(), o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase("Dummy");
                o.ModelConstructor = mb => mb.AddAccount();
            });
            var sut4 = new EfCoreRepository<Account, long>(sut3);

            var id = Guid.NewGuid();
            var name = "Test";
            var email = "test@unit.test";

            sut4.Add(new Account(id, name, email));
            await sut3.SaveChangesAsync(); // should raise domain events

            Assert.Equal(id, sut2.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<AccountInitiated>().Single().PlatformProviderId);
            Assert.Equal(name, sut2.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<AccountInitiated>().Single().FullName);
            Assert.Equal(email, sut2.GetRequiredService<ITestStore<IDomainEvent>>().QueryFor<AccountInitiated>().Single().EmailAddress);
        }
    }
}
