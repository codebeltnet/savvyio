---
uid: Savvyio.EventDriven.IntegrationEventDispatcher
example:
- *content
---
`IntegrationEventDispatcher` routes an integration event to all registered `IIntegrationEventHandler` implementations through a `ServiceLocator`. Register the handler and create the dispatcher with a service locator that resolves handlers by their service type.

```csharp
using System.Collections.Generic;
using Savvyio;
using Savvyio.Dispatchers;
using Savvyio.EventDriven;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class IntegrationEventDispatcherExample
{
    public IReadOnlyCollection<string> Publish()
    {
        var handler = new AccountCreatedHandler();
        var locator = new ServiceLocator(serviceType =>
            serviceType == typeof(IIntegrationEventHandler) ? new object[] { handler } : new object[0]);
        var dispatcher = new IntegrationEventDispatcher(locator);
        dispatcher.Publish(new AccountCreatedEvent("ACC-42", "alice@example.com"));
        return handler.NotifiedEmails;
    }
}

public sealed class AccountCreatedHandler : IIntegrationEventHandler
{
    public List<string> NotifiedEmails { get; } = new();

    public IFireForgetActivator<IIntegrationEvent> Delegates =>
        HandlerFactory.CreateFireForget<IIntegrationEvent>(r =>
            r.Register<AccountCreatedEvent>(e => NotifiedEmails.Add(e.Email)));
}

public sealed record AccountCreatedEvent(string AccountId, string Email) : Request, IIntegrationEvent;
```
