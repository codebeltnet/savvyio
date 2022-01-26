using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savvyio.EventDriven;
using Savvyio.Handlers;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.Assets
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
