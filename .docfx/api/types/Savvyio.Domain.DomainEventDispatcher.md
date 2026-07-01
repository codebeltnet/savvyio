---
uid: Savvyio.Domain.DomainEventDispatcher
example:
- *content
---
This example shows how to route an in-process domain event to a registered handler through the default domain event dispatcher. The setup keeps the aggregate logic isolated while the handler records which account events were observed.

```csharp
using System.Collections.Generic;
using Savvyio;
using Savvyio.Dispatchers;
using Savvyio.Domain;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class DomainEventDispatcherExample
{
    public IReadOnlyCollection<string> Raise()
    {
        var handler = new AccountOpenedHandler();
        var dispatcher = new DomainEventDispatcher(new ServiceLocator(serviceType => serviceType == typeof(IDomainEventHandler) ? new object[] { handler } : []));
        dispatcher.Raise(new AccountOpenedEvent("ACC-42"));
        return handler.ProcessedAccounts;
    }
}

public sealed class AccountOpenedHandler : IDomainEventHandler
{
    public List<string> ProcessedAccounts { get; } = new();
    public IFireForgetActivator<IDomainEvent> Delegates => HandlerFactory.CreateFireForget<IDomainEvent>(registry => registry.Register<AccountOpenedEvent>(e => ProcessedAccounts.Add(e.AccountId)));
}

public sealed record AccountOpenedEvent(string AccountId) : Request, IDomainEvent;
```
