---
uid: Savvyio.Extensions.Mediator
example:
- *content
---
`Mediator` is the single-entry-point dispatcher that routes commands, queries, and events to their handlers without requiring separate dispatcher injections. Create it with a `ServiceLocator` that resolves handlers by service type.

```csharp
using System;
using System.Collections.Generic;
using Savvyio;
using Savvyio.Commands;
using Savvyio.Dispatchers;
using Savvyio.Extensions;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class MediatorExample
{
    public void Dispatch()
    {
        var handler = new CreateOrderHandler();
        var locator = new ServiceLocator(serviceType =>
            serviceType == typeof(ICommandHandler) ? new object[] { handler } : Array.Empty<object>());
        var mediator = new Mediator(locator);

        mediator.Commit(new CreateOrderCommand("SO-42"));
        Console.WriteLine($"Processed orders: {handler.ProcessedOrders.Count}");
    }
}

public sealed class CreateOrderHandler : ICommandHandler
{
    public List<string> ProcessedOrders { get; } = new();

    public IFireForgetActivator<ICommand> Delegates =>
        HandlerFactory.CreateFireForget<ICommand>(r =>
            r.Register<CreateOrderCommand>(cmd => ProcessedOrders.Add(cmd.OrderId)));
}

public sealed record CreateOrderCommand(string OrderId) : Request, ICommand;
```

