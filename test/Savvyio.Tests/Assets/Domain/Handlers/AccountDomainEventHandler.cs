using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain;
using Xunit.Abstractions;

namespace Savvyio.Assets.Domain.Handlers
{
    public class AccountDomainEventHandler : DomainEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IDomainEvent> _testStore;

        public AccountDomainEventHandler(ITestOutputHelper output, ITestStore<IDomainEvent> testStore)
        {
            _output = output;
            _testStore = testStore;
        }

        protected override void RegisterDomainEventHandlers(IHandlerRegistry<IDomainEvent> handler)
        {
            handler.RegisterAsync<AccountInitiated>(OnInProcAccountCreated);
            handler.RegisterAsync<AccountEmailAddressChanged>(OnInProcAccountEmailAddressChanged);
            handler.RegisterAsync<AccountFullNameChanged>(OnInProcAccountFullNameChanged);
        }

        private Task OnInProcAccountFullNameChanged(AccountFullNameChanged e)
        {
            _testStore.Add(e);
            _output.WriteLines($"DE {nameof(OnInProcAccountFullNameChanged)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private Task OnInProcAccountEmailAddressChanged(AccountEmailAddressChanged e)
        {
            _testStore.Add(e);
            _output.WriteLines($"DE {nameof(OnInProcAccountEmailAddressChanged)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        private Task OnInProcAccountCreated(AccountInitiated e)
        {
            _testStore.Add(e);
            _output.WriteLines($"DE {nameof(OnInProcAccountCreated)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }
    }
}
