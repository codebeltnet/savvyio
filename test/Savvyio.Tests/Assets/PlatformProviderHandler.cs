using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Cuemon.Threading;
using Savvyio.Assets.Commands;
using Savvyio.Assets.Domain;
using Savvyio.Assets.Events;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Events;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class PlatformProviderHandler : ICommandHandler, IIntegrationEventHandler
    {
        private readonly IMediator _mediator;
        private readonly ITestOutputHelper _output;
        private readonly IActiveRecordRepository<PlatformProvider, Guid> _activeRecordRepository;
        private readonly ITestStore<IIntegrationEvent> _testStore;

        public PlatformProviderHandler(IMediator mediator, ITestOutputHelper output, ITestStore<IIntegrationEvent> testStore, IActiveRecordRepository<PlatformProvider, Guid> activeRecordRepository)
        {
            _mediator = mediator;
            _output = output;
            _testStore = testStore;
            _activeRecordRepository = activeRecordRepository;
        }

        //public IFireForgetActivator<ICommand> Commands => HandlerFactory.CreateFireForget<ICommand>(handler =>
        //{
        //    handler.RegisterAsync<CreatePlatformProvider>(c =>
        //    {
        //        var provider = new PlatformProvider(c.Name, c.ThirdLevelDomainName, c.Description);
        //        _activeRecordRepository.SaveAsync(provider); // store in db
        //        return _mediator.PublishAsync(new PlatformProviderCreated(provider));
        //    });

        //    handler.RegisterAsync<UpdatePlatformProvider>(async c =>
        //    {
        //        var provider = new PlatformProvider(c.Id);
        //        provider.ChangeDescription(c.Description);
        //        provider.ChangeName(c.Name);
        //        provider.ChangeThirdLevelDomainName(c.ThirdLevelDomainName);
        //        await _activeRecordRepository.SaveAsync(provider); // store in db
        //        await _mediator.PublishAsync(new PlatformProviderUpdated(provider));
        //    });

        //    handler.RegisterAsync<UpdatePlatformProviderAccountPolicy>(async c =>
        //    {
        //        var provider = new PlatformProvider(c.Id);
        //        provider.ChangeAccountPolicy(new PlatformProviderAccountPolicy(c.MinimumPasswordLength, c.EnforcePasswordHistory, c.MinimumPasswordAge, c.MaximumPasswordAge, c.AccountLockoutThreshold, c.AccountLockoutDuration, c.AccountLockoutCounterReset, c.PasswordComplexityRequirement));
        //        await _activeRecordRepository.SaveAsync(provider);
        //        await _mediator.PublishAsync(new PlatformProviderUpdated(provider));
        //    });
        //});

        //public IFireForgetActivator<IIntegrationEvent> IntegrationEvents => HandlerFactory.CreateFireForget<IIntegrationEvent>(handler =>
        //{
        //    handler.RegisterAsync<PlatformProviderCreated>(e =>
        //    {
        //        _testStore.Add(e);
        //        _output.WriteLines($"IE {nameof(PlatformProviderCreated)}", JsonSerializer.Serialize(e));
        //        return Task.CompletedTask;
        //    });

        //    handler.RegisterAsync<PlatformProviderUpdated>(e =>
        //    {
        //        _testStore.Add(e);
        //        _output.WriteLines($"IE {nameof(PlatformProviderUpdated)}", JsonSerializer.Serialize(e));
        //        return Task.CompletedTask;
        //    });
        //});

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
            handler.RegisterAsync<CreatePlatformProvider>(c =>
            {
                _output.WriteLines($"C {nameof(CreatePlatformProvider)}", JsonSerializer.Serialize(c));
                var provider = new PlatformProvider(c.Name, c.ThirdLevelDomainName, c.Description).MergeMetadata(c);
                _activeRecordRepository.SaveAsync(provider); // store in db
                return _mediator.PublishAsync(new PlatformProviderCreated(provider).MergeMetadata(c));
            });

            handler.RegisterAsync<UpdatePlatformProvider>(async c =>
            {
                var provider = new PlatformProvider(c.Id);
                provider.ChangeDescription(c.Description);
                provider.ChangeName(c.Name);
                provider.ChangeThirdLevelDomainName(c.ThirdLevelDomainName);
                await _activeRecordRepository.SaveAsync(provider); // store in db
                await _mediator.PublishAsync(new PlatformProviderUpdated(provider));
            });

            handler.RegisterAsync<UpdatePlatformProviderAccountPolicy>(async c =>
            {
                var provider = new PlatformProvider(c.Id);
                provider.ChangeAccountPolicy(new PlatformProviderAccountPolicy(c.MinimumPasswordLength, c.EnforcePasswordHistory, c.MinimumPasswordAge, c.MaximumPasswordAge, c.AccountLockoutThreshold, c.AccountLockoutDuration, c.AccountLockoutCounterReset, c.PasswordComplexityRequirement));
                await _activeRecordRepository.SaveAsync(provider);
                await _mediator.PublishAsync(new PlatformProviderUpdated(provider));
            });
        });
    }
}
