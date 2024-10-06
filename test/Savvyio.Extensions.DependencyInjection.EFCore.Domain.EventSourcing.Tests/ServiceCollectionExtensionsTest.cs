using System;
using Codebelt.Extensions.Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Domain.EventSourcing;
using Savvyio.Domain;
using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.Domain.EventSourcing;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Extensions.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing
{
    public class ServiceCollectionExtensionsTest  : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddEfCoreTracedAggregateRepository_ShouldHaveTypeForwardedImplementations()
        {
            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<NewtonsoftJsonMarshaller>();
            sut1.AddEfCoreDataSource(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddEfCoreTracedAggregateRepository<TracedAccount, Guid>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreTracedAggregateRepository<TracedAccount, Guid>>(sut2.GetRequiredService<ITracedAggregateRepository<TracedAccount, Guid>>());
            Assert.IsType<EfCoreTracedAggregateRepository<TracedAccount, Guid>>(sut2.GetRequiredService<IReadableRepository<TracedAccount, Guid>>());
            Assert.IsType<EfCoreTracedAggregateRepository<TracedAccount, Guid>>(sut2.GetRequiredService<IWritableRepository<TracedAccount, Guid>>());
        }

        [Fact]
        public void AddEfCoreTracedAggregateRepository_ShouldHaveTypeForwardedImplementationsWithMarker()
        {
            var sut1 = new ServiceCollection();
            sut1.AddMarshaller<JsonMarshaller>();
            sut1.AddEfCoreDataSource<DbMarker>(o => o.ContextConfigurator = b => b.UseInMemoryDatabase("fake"));
            sut1.AddEfCoreTracedAggregateRepository<TracedAccount, Guid, DbMarker>();
            var sut2 = sut1.BuildServiceProvider();

            Assert.IsType<EfCoreTracedAggregateRepository<TracedAccount, Guid, DbMarker>>(sut2.GetRequiredService<ITracedAggregateRepository<TracedAccount, Guid, DbMarker>>());
            Assert.IsType<EfCoreTracedAggregateRepository<TracedAccount, Guid, DbMarker>>(sut2.GetRequiredService<IReadableRepository<TracedAccount, Guid, DbMarker>>());
            Assert.IsType<EfCoreTracedAggregateRepository<TracedAccount, Guid, DbMarker>>(sut2.GetRequiredService<IWritableRepository<TracedAccount, Guid, DbMarker>>());
        }
    }
}
