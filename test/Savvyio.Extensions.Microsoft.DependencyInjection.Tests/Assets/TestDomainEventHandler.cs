using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savvyio.Domain;
using Savvyio.Handlers;

namespace Savvyio.Extensions.Microsoft.DependencyInjection.Assets
{
    public class TestDomainEventHandler : DomainEventHandler
    {
        protected override void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers)
        {
            handlers.Register<TestDomainEvent>(delegate(TestDomainEvent _) {  });
        }
    }
}
