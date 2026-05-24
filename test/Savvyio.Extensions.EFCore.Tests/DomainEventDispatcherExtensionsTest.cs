using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Savvyio.Assets;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain;
using Xunit;

namespace Savvyio.Extensions.EFCore.Domain
{
    public class DomainEventDispatcherExtensionsTest : Test
    {
        public DomainEventDispatcherExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DomainEventDispatcherExtensions_ShouldRaiseManySynchronously()
        {
            var dispatcher = new TrackingDomainEventDispatcher();
            var source = new EfCoreAggregateDataSource(null, new EfCoreDataSourceOptions
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("dispatcher-" + Guid.NewGuid()),
                ModelConstructor = mb => mb.AddAccount()
            });
            var repository = new EfCoreRepository<Account, long>(source);
            var id = Guid.NewGuid();

            repository.Add(new Account(id, "Test", "test@unit.test"));
            dispatcher.RaiseMany(source.DbContext);

            var tracked = source.DbContext.ChangeTracker.Entries<IAggregateRoot<IDomainEvent>>().Single().Entity;

            Assert.Single(dispatcher.Events);
            Assert.IsType<AccountInitiated>(dispatcher.Events.Single());
            Assert.Equal(id, ((AccountInitiated)dispatcher.Events.Single()).PlatformProviderId);
            Assert.Empty(tracked.Events);
        }

        [Fact]
        public async Task EfCoreAggregateDataSource_ShouldSaveChangesWithoutDispatcher()
        {
            var id = Guid.NewGuid();
            var source = new EfCoreAggregateDataSource(null, new EfCoreDataSourceOptions
            {
                ContextConfigurator = b => b.UseInMemoryDatabase("aggregate-null-dispatcher-" + Guid.NewGuid()),
                ModelConstructor = mb => mb.AddAccount()
            });
            var repository = new EfCoreRepository<Account, long>(source);

            repository.Add(new Account(id, "Test", "test@unit.test"));
            await source.SaveChangesAsync();

            var sut = await repository.FindAllAsync(a => a.PlatformProviderId == id).SingleOrDefaultAsync();

            Assert.NotNull(sut);
            Assert.Equal(id, sut.PlatformProviderId);
        }

        private sealed class TrackingDomainEventDispatcher : IDomainEventDispatcher
        {
            public List<IDomainEvent> Events { get; } = new();

            public void Raise(IDomainEvent request)
            {
                Events.Add(request);
            }

            public Task RaiseAsync(IDomainEvent request, Action<Cuemon.Threading.AsyncOptions> setup = null)
            {
                Events.Add(request);
                return Task.CompletedTask;
            }
        }
    }
}
