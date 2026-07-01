---
uid: Savvyio.EventDriven.SavvyioOptionsExtensions
example:
- *content
---
This example shows how to register an integration-event handler together with the default integration event dispatcher.

```csharp
using Savvyio;
using Savvyio.EventDriven;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class EventDrivenOptionsExtensionsExample
{
    public SavvyioOptions Configure() => new SavvyioOptions().AddIntegrationEventHandler<MemberCreatedHandler>().AddIntegrationEventDispatcher();
}

public sealed class MemberCreatedHandler : IIntegrationEventHandler
{
    public IFireForgetActivator<IIntegrationEvent> Delegates => HandlerFactory.CreateFireForget<IIntegrationEvent>(registry => registry.Register<MemberCreatedEvent>(_ => { }));
}

public sealed record MemberCreatedEvent(string MemberId) : Request, IIntegrationEvent;
```
