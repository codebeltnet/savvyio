using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Events;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.EventDriven;
using Savvyio.Extensions.Dapper;
using Savvyio.Handlers;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class AccountEventHandler : IntegrationEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IIntegrationEvent> _testStore;
        private readonly IPersistentDataStore<PlatformProviderCreated, DapperQueryOptions> _platformProviderDao;
        private readonly IPersistentDataStore<AccountProjection, DapperQueryOptions> _accountDao;

        public AccountEventHandler(ITestOutputHelper output = null, ITestStore<IIntegrationEvent> testStore = null, IPersistentDataStore<AccountProjection, DapperQueryOptions> accountDao = null, IPersistentDataStore<PlatformProviderCreated, DapperQueryOptions> platformProviderDao = null)
        {
            _output = output;
            _testStore = testStore;
            _accountDao = accountDao;
            _platformProviderDao = platformProviderDao;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers)
        {
            handlers.RegisterAsync<AccountCreated>(OnOutProcAccountCreated);
            handlers.RegisterAsync<AccountUpdated>(OnOutProcAccountUpdated);
        }

        private Task OnOutProcAccountUpdated(AccountUpdated e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"IE {nameof(OnOutProcAccountUpdated)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        public Task OnOutProcAccountCreated(AccountCreated e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"IE {nameof(OnOutProcAccountCreated)}", JsonSerializer.Serialize(e));
            _accountDao?.CreateAsync(new AccountProjection(e.Id, e.FullName, e.EmailAddress));
            return Task.CompletedTask;
        }
    }
}
