using Savvyio.Domain;
using Savvyio.Handlers;

namespace Savvyio.Extensions.Assets
{
    public class TestDomainEventHandler : DomainEventHandler
    {
        protected override void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers)
        {
            handlers.Register<TestDomainEvent>(delegate(TestDomainEvent _) {  });
        }
    }
}
