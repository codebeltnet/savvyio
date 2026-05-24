using System;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Domain;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore;
using Savvyio.Extensions.EFCore.Domain;
using Xunit;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain
{
    public class AggregateDataSourceRegistrationTest : Test
    {
        public AggregateDataSourceRegistrationTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddEfCoreAggregateDataSource_ShouldTypeForwardDefaultImplementation()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDomainEventDispatcher, SilentDomainEventDispatcher>();
            services.AddEfCoreAggregateDataSource(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("aggregate-" + Guid.NewGuid()));
            var provider = services.BuildServiceProvider();

            Assert.IsType<EfCoreAggregateDataSource>(provider.GetRequiredService<IEfCoreDataSource>());
            Assert.IsType<EfCoreAggregateDataSource>(provider.GetRequiredService<IDataSource>());
            Assert.IsType<EfCoreAggregateDataSource>(provider.GetRequiredService<IUnitOfWork>());
        }

        [Fact]
        public void AddEfCoreAggregateDataSource_ShouldTypeForwardMarkedImplementation()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDomainEventDispatcher, SilentDomainEventDispatcher>();
            services.AddEfCoreAggregateDataSource<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("aggregate-marker-" + Guid.NewGuid()));
            var provider = services.BuildServiceProvider();

            Assert.IsType<EfCoreAggregateDataSource<DbMarker>>(provider.GetRequiredService<IEfCoreDataSource<DbMarker>>());
            Assert.IsType<EfCoreAggregateDataSource<DbMarker>>(provider.GetRequiredService<IDataSource<DbMarker>>());
            Assert.IsType<EfCoreAggregateDataSource<DbMarker>>(provider.GetRequiredService<IUnitOfWork<DbMarker>>());
        }

        private sealed class SilentDomainEventDispatcher : IDomainEventDispatcher
        {
            public void Raise(IDomainEvent request)
            {
            }

            public Task RaiseAsync(IDomainEvent request, Action<AsyncOptions> setup = null)
            {
                return Task.CompletedTask;
            }
        }
    }
}
