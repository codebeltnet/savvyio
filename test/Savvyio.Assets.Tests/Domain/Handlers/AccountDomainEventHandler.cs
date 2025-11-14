using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Domain.Events;
using Savvyio.Assets.Queries;
using Savvyio.Data;
using Savvyio.Domain;
using Savvyio.Extensions.Dapper;
using Savvyio.Handlers;
using Xunit;

namespace Savvyio.Assets.Domain.Handlers
{
    public class AccountDomainEventHandler : DomainEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IDomainEvent> _testStore;
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly ISearchableDataStore<AccountProjection, DapperQueryOptions> _accountDao;

        public AccountDomainEventHandler(ITestOutputHelper output = null, ITestStore<IDomainEvent> testStore = null, IDomainEventDispatcher dispatcher = null, ISearchableDataStore<AccountProjection, DapperQueryOptions> accountDao = null)
        {
            _output = output;
            _testStore = testStore;
            _dispatcher = dispatcher;
            _accountDao = accountDao;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers)
        {
            handlers.RegisterAsync<AccountInitiated>(OnInProcAccountInitiated);
            handlers.RegisterAsync<AccountEmailAddressChanged>(OnInProcAccountEmailAddressChanged);
            handlers.RegisterAsync<AccountFullNameChanged>(OnInProcAccountFullNameChanged);
            handlers.RegisterAsync<AccountInitiatedChained>(OnInProcAccountInitiatedChained);
            handlers.RegisterAsync<TracedAccountInitiated>(OnInProcTracedAccountInitiated);
            handlers.RegisterAsync<TracedAccountEmailAddressChanged>(OnInProcTracedAccountEmailAddressChanged);
        }

        private Task OnInProcTracedAccountInitiated(TracedAccountInitiated e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcTracedAccountInitiated)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private Task OnInProcTracedAccountEmailAddressChanged(TracedAccountEmailAddressChanged e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcTracedAccountEmailAddressChanged)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private Task OnInProcAccountInitiatedChained(AccountInitiatedChained e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcAccountInitiatedChained)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private Task OnInProcAccountFullNameChanged(AccountFullNameChanged e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcAccountFullNameChanged)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private Task OnInProcAccountEmailAddressChanged(AccountEmailAddressChanged e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcAccountEmailAddressChanged)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private async Task OnInProcAccountInitiated(AccountInitiated e)
        {
            if (_accountDao != null)
            {
                var dao = await _accountDao.FindAllAsync(o =>
                {
                    o.CommandText = "SELECT * FROM AccountProjection WHERE EmailAddress = @EmailAddress";
                    o.Parameters = new { e.EmailAddress };
                }).SingleOrDefaultAsync().ConfigureAwait(false);

                if (dao != null) { throw new ValidationException("Email address has already been registered."); }
            }

            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcAccountInitiated)}", JsonSerializer.Serialize(e));
            await _dispatcher.RaiseAsync(new AccountInitiatedChained().MergeMetadata(e).SetCausationId(e.GetEventId())).ConfigureAwait(false);
        }
    }
}
