using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Events;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.EventDriven;
using Savvyio.EventDriven.Messaging;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.SimpleQueueService.EventDriven;
using Savvyio.Handlers;
using Savvyio.Messaging;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class AccountEventHandler : IntegrationEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IIntegrationEvent> _testStore;
        private readonly IPersistentDataStore<PlatformProviderCreated, DapperQueryOptions> _platformProviderDao;
        private readonly IPersistentDataStore<AccountProjection, DapperQueryOptions> _accountDao;
        private readonly IPublisher<IIntegrationEvent> _eventBus;

        public AccountEventHandler(ITestOutputHelper output = null, ITestStore<IIntegrationEvent> testStore = null, IPersistentDataStore<AccountProjection, DapperQueryOptions> accountDao = null, IPersistentDataStore<PlatformProviderCreated, DapperQueryOptions> platformProviderDao = null, IPublisher<IIntegrationEvent> eventBus = null)
        {
            _output = output;
            _testStore = testStore;
            _accountDao = accountDao;
            _platformProviderDao = platformProviderDao;
            _eventBus = eventBus;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers)
        {
            handlers.RegisterAsync<AccountCreated>(OnOutProcAccountCreated);
            handlers.RegisterAsync<AccountUpdated>(OnOutProcAccountUpdated);
        }

        private Task OnOutProcAccountUpdated(AccountUpdated e)
        {
            _testStore?.Add(e);
            _eventBus?.PublishAsync(e.ToMessage("urn:event:account-updated".ToSnsUri()));
            _output?.WriteLines($"IE {nameof(OnOutProcAccountUpdated)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        public async Task OnOutProcAccountCreated(AccountCreated e)
        {
            _testStore?.Add(e);
            if (_eventBus != null)
            {
                await _eventBus.PublishAsync(e.ToMessage("account-created-flow-test.fifo".ToSnsUri()));
            }
            _output?.WriteLines($"IE {nameof(OnOutProcAccountCreated)}", JsonSerializer.Serialize(e));
            if (_accountDao != null)
            {
                await _accountDao.CreateAsync(new AccountProjection(e.Id, e.FullName, e.EmailAddress));
            }
        }
    }
}
