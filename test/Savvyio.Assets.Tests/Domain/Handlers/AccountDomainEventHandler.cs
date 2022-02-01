using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain;
using Savvyio.Handlers;
using Xunit.Abstractions;

namespace Savvyio.Assets.Domain.Handlers
{
    public class AccountDomainEventHandler : DomainEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IDomainEvent> _testStore;
        private readonly IDomainEventDispatcher _dispatcher;

        public AccountDomainEventHandler(ITestOutputHelper output = null, ITestStore<IDomainEvent> testStore = null, IDomainEventDispatcher dispatcher = null)
        {
            _output = output;
            _testStore = testStore;
            _dispatcher = dispatcher;
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

        private Task OnInProcAccountInitiated(AccountInitiated e)
        {
            _testStore?.Add(e);
            _output?.WriteLines($"DE {nameof(OnInProcAccountInitiated)}", JsonSerializer.Serialize(e));
            _dispatcher.RaiseAsync(new AccountInitiatedChained().MergeMetadata(e).SetCausationId(e.GetEventId()));
            return Task.CompletedTask;
        }
    }
}
