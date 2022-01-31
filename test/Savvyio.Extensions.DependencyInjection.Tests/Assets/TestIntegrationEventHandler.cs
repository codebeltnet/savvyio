using System;
using Savvyio.EventDriven;
using Savvyio.Handlers;

namespace Savvyio.Extensions.Assets
{
    internal class TestIntegrationEventHandler : IntegrationEventHandler
    {
        protected override void RegisterDelegates(IFireForgetRegistry<IIntegrationEvent> handlers)
        {
            handlers.Register<TestIntegrationEvent>(Handler);
        }

        private void Handler(TestIntegrationEvent obj)
        {
            throw new NotImplementedException();
        }
    }
}
