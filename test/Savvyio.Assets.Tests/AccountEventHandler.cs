﻿using System.Text.Json;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Savvyio.Assets.Events;
using Savvyio.EventDriven;
using Savvyio.Handlers;
using Xunit.Abstractions;

namespace Savvyio.Assets
{
    public class AccountEventHandler : IntegrationEventHandler
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestStore<IIntegrationEvent> _testStore;

        public AccountEventHandler(ITestOutputHelper output, ITestStore<IIntegrationEvent> testStore)
        {
            _output = output;
            _testStore = testStore;
        }

        protected override void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers)
        {
            handlers.RegisterAsync<AccountCreated>(OnOutProcAccountCreated);
            handlers.RegisterAsync<AccountUpdated>(OnOutProcAccountUpdated);
        }

        private Task OnOutProcAccountUpdated(AccountUpdated e)
        {
            _testStore.Add(e);
            _output.WriteLines($"IE {nameof(OnOutProcAccountUpdated)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }

        public Task OnOutProcAccountCreated(AccountCreated e)
        {
            _testStore.Add(e);
            _output.WriteLines($"IE {nameof(OnOutProcAccountCreated)}", JsonSerializer.Serialize(e));
            return Task.CompletedTask;
        }
    }
}