using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Assets;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.Extensions;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Dapper;
using Savvyio.Extensions.DependencyInjection.DapperExtensions;
using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.DependencyInjection.EFCore;
using Savvyio.Extensions.DependencyInjection.EFCore.Domain;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Savvyio
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class MediatorTest : HostTest<HostFixture>
    {
        private readonly IServiceProvider _sp;

        public MediatorTest(HostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
        {
            _sp = hostFixture.ServiceProvider;
        }

        [Fact, Priority(0)]
        public async Task RegisterNewUser()
        {
            var correlationId = Guid.NewGuid().ToString("N");
            var caPpId = Guid.NewGuid();
            var caFullName = "Michael Mortensen";
            var caEmailAddress = "root@gimlichael.dev";

            var mediator = _sp.GetRequiredService<IMediator>();
            
            var hsd = _sp.GetRequiredService<HandlerServicesDescriptor>();
            TestOutput.WriteLine(hsd.ToString());

            var accountRepo = _sp.GetRequiredService<ISearchableRepository<Account, long, Account>>();
            
            await mediator.CommitAsync(new CreateAccount(caPpId, caFullName, caEmailAddress).SetCorrelationId(correlationId));

            var entity = await accountRepo.FindAllAsync(a => a.Metadata.Contains(new KeyValuePair<string, object>(MetadataDictionary.CorrelationId, correlationId))).SingleOrDefaultAsync();

            var dao = await mediator.QueryAsync(new GetAccount(entity.Id)).ConfigureAwait(false);

            Assert.Equal(entity.Id, dao.Id);
            Assert.Equal(entity.EmailAddress, dao.EmailAddress);
            Assert.Equal(entity.FullName, dao.FullName);

        }

        [Fact, Priority(1)]
        public async Task RegisterNewUser_ShouldFailWithValidationException_BecauseOfUniqueEmailAddress()
        {
            var correlationId = Guid.NewGuid().ToString("N");
            var caPpId = Guid.NewGuid();
            var caFullName = "El Presidente";
            var caEmailAddress = "root@gimlichael.dev";

            var mediator = _sp.GetRequiredService<IMediator>();
            
            await Assert.ThrowsAsync<ValidationException>(() => mediator.CommitAsync(new CreateAccount(caPpId, caFullName, caEmailAddress).SetCorrelationId(correlationId)));
        }

        [Fact, Priority(2)]
        public async Task RegisterAnotherUser()
        {
            var caPpId = Guid.NewGuid();
            var caFullName = "El Presidento";
            var caEmailAddress = "makemyday@us.gov";

            var mediator = _sp.GetRequiredService<IMediator>();
            
            await mediator.CommitAsync(new CreateAccount(caPpId, caFullName, caEmailAddress));
        }

        [Fact, Priority(3)]
        public async Task VerifyUsers()
        {
            var accountRepo = _sp.GetRequiredService<ISearchableRepository<Account, long, Account>>();
            var accountDao = _sp.GetRequiredService<ISearchableDataStore<AccountProjection, DapperQueryOptions>>();
            var daos = new List<AccountProjection>(await accountDao.FindAllAsync().ConfigureAwait(false));
            var entities = new List<Account>(await accountRepo.FindAllAsync().ConfigureAwait(false));
            foreach (var entity in entities)
            {
                TestOutput.WriteLine(entity.EmailAddress);
                Assert.Equal(entity.Id, daos.Single(ac => ac.Id == entity.Id).Id);
                Assert.Equal(entity.EmailAddress, daos.Single(ac => ac.Id == entity.Id).EmailAddress);
                Assert.Equal(entity.FullName, daos.Single(ac => ac.Id == entity.Id).FullName);
            }

            Assert.Equal(2, entities.Count);
            Assert.Equal(2, daos.Count);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddEfCoreAggregateDataSource<Account>(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(Account)).EnableDetailedErrors().LogTo(Console.WriteLine);
                o.ModelConstructor = mb => mb.AddAccount();
            }).AddEfCoreRepository<Account, long, Account>();

            services.AddEfCoreAggregateDataSource<PlatformProvider>(o =>
            {
                o.ContextConfigurator = b => b.UseInMemoryDatabase(nameof(PlatformProvider)).EnableDetailedErrors().LogTo(Console.WriteLine);
                o.ModelConstructor = mb => mb.AddPlatformProvider();
            }).AddEfCoreRepository<PlatformProvider, Guid, PlatformProvider>();

            services.AddDapperDataSource(o => o.ConnectionFactory = () => new SqliteConnection().SetDefaults().AddAccountTable().AddPlatformProviderTable(), o => o.Lifetime = ServiceLifetime.Scoped)
                .AddDapperDataStore<AccountData, AccountProjection>()
                .AddDapperExtensionsDataStore<PlatformProviderProjection>();

            services.AddSavvyIO(o =>
            {
                o.EnableHandlerServicesDescriptor().UseAutomaticDispatcherDiscovery().UseAutomaticHandlerDiscovery().AddMediator<Mediator>();
            });
        }
    }
}
