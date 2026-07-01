---
uid: Savvyio.Domain.SavvyioOptionsExtensions
example:
- *content
---
This example shows how to register a domain-event handler together with the default domain event dispatcher.

```csharp
using Savvyio;
using Savvyio.Domain;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class DomainOptionsExtensionsExample
{
    public SavvyioOptions Configure() => new SavvyioOptions().AddDomainEventHandler<AccountOpenedHandler>().AddDomainEventDispatcher();
}

public sealed class AccountOpenedHandler : IDomainEventHandler
{
    public IFireForgetActivator<IDomainEvent> Delegates => HandlerFactory.CreateFireForget<IDomainEvent>(registry => registry.Register<AccountOpenedEvent>(_ => { }));
}

public sealed record AccountOpenedEvent(string AccountId) : Request, IDomainEvent;
```
