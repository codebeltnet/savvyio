using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Domain.Events;
using Savvyio.Domain;
using Savvyio.Handlers;
using Xunit.Abstractions;

namespace Savvyio.Assets.Domain.Handlers
{
    public class PlatformProviderDomainEventHandler : DomainEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IDomainEvent> _testStore;

        public PlatformProviderDomainEventHandler(ITestOutputHelper output, ITestStore<IDomainEvent> testStore)
        {
            _output = output;
            _testStore = testStore;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers)
        {
            handlers.RegisterAsync<PlatformProviderAccountPolicyChanged>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"DE {nameof(PlatformProviderAccountPolicyChanged)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });
            handlers.RegisterAsync<PlatformProviderInitiated>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"DE {nameof(PlatformProviderInitiated)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });
            handlers.RegisterAsync<PlatformProviderDescriptionChanged>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"DE {nameof(PlatformProviderDescriptionChanged)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });
            handlers.RegisterAsync<PlatformProviderNameChanged>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"DE {nameof(PlatformProviderNameChanged)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });
            handlers.RegisterAsync<PlatformProviderThirdLevelDomainNameChanged>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"DE {nameof(PlatformProviderThirdLevelDomainNameChanged)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });
        }
    }
}
