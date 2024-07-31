using Savvyio.Domain;
using Savvyio.Handlers;

namespace Savvyio.Extensions.DependencyInjection.Assets
{
    public class TestDomainEventHandler : DomainEventHandler
    {
        protected override void RegisterDelegates(IFireForgetRegistry<IDomainEvent> handlers)
        {
            handlers.Register<TestDomainEvent>(delegate(TestDomainEvent _) {  });
        }
    }
}
