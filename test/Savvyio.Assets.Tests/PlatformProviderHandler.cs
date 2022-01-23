using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Events;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.Dispatchers;
using Savvyio.Extensions.Storage;
using Savvyio.Handlers;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class PlatformProviderHandler : ICommandHandler, IIntegrationEventHandler
    {
        private readonly IMediator _mediator;
        private readonly ITestOutputHelper _output;
        private readonly IPersistentDataAccessObject<PlatformProvider, PlatformProvider> _activeRecordRepository;
        private readonly ITestStore<IIntegrationEvent> _testStore;

        public PlatformProviderHandler(IMediator mediator, ITestOutputHelper output, ITestStore<IIntegrationEvent> testStore, IPersistentDataAccessObject<PlatformProvider, PlatformProvider> activeRecordRepository)
        {
            _mediator = mediator;
            _output = output;
            _testStore = testStore;
            _activeRecordRepository = activeRecordRepository;
        }

        IFireForgetActivator<IIntegrationEvent> IFireForgetHandler<IIntegrationEvent>.Delegates => HandlerFactory.CreateFireForget<IIntegrationEvent>(handler =>
        {
            handler.RegisterAsync<PlatformProviderCreated>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"IE {nameof(PlatformProviderCreated)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });

            handler.RegisterAsync<PlatformProviderUpdated>(e =>
            {
                _testStore.Add(e);
                _output.WriteLines($"IE {nameof(PlatformProviderUpdated)}", JsonSerializer.Serialize(e));
                return Task.CompletedTask;
            });
        });

        IFireForgetActivator<ICommand> IFireForgetHandler<ICommand>.Delegates => HandlerFactory.CreateFireForget<ICommand>(handler =>
        {
            handler.RegisterAsync<CreatePlatformProvider>(async c =>
            {
                _output.WriteLines($"C {nameof(CreatePlatformProvider)}", JsonSerializer.Serialize(c));
                var provider = new PlatformProvider(c.Name, c.ThirdLevelDomainName, c.Description).MergeMetadata(c);
                await _activeRecordRepository.CreateAsync(provider); // store in db
                await _mediator.PublishAsync(new PlatformProviderCreated(provider).MergeMetadata(c));
            });

            handler.RegisterAsync<UpdatePlatformProvider>(async c =>
            {
                var provider = new PlatformProvider(c.Id);
                provider.ChangeDescription(c.Description);
                provider.ChangeName(c.Name);
                provider.ChangeThirdLevelDomainName(c.ThirdLevelDomainName);
                await _activeRecordRepository.UpdateAsync(provider); // store in db
                await _mediator.PublishAsync(new PlatformProviderUpdated(provider));
            });

            handler.RegisterAsync<UpdatePlatformProviderAccountPolicy>(async c =>
            {
                var provider = new PlatformProvider(c.Id);
                provider.ChangeAccountPolicy(new PlatformProviderAccountPolicy(c.MinimumPasswordLength, c.EnforcePasswordHistory, c.MinimumPasswordAge, c.MaximumPasswordAge, c.AccountLockoutThreshold, c.AccountLockoutDuration, c.AccountLockoutCounterReset, c.PasswordComplexityRequirement));
                await _activeRecordRepository.UpdateAsync(provider);
                await _mediator.PublishAsync(new PlatformProviderUpdated(provider));
            });
        });
    }
}
