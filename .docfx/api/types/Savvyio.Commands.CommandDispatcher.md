---
uid: Savvyio.Commands.CommandDispatcher
example:
- *content
---
This example shows how to wire the built-in command dispatcher to a concrete command handler through ServiceLocator. The setup mirrors a mediator pipeline where the handler tracks processed order identifiers and the dispatcher invokes it with a fire-and-forget command.

```csharp
using System.Collections.Generic;
using Savvyio;
using Savvyio.Commands;
using Savvyio.Dispatchers;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class CommandDispatcherExample
{
    public IReadOnlyCollection<string> Commit()
    {
        var handler = new CreateOrderHandler();
        var dispatcher = new CommandDispatcher(new ServiceLocator(serviceType => serviceType == typeof(ICommandHandler) ? new object[] { handler } : []));
        dispatcher.Commit(new CreateOrderCommand("ORD-42"));
        return handler.ProcessedOrders;
    }
}

public sealed class CreateOrderHandler : ICommandHandler
{
    public List<string> ProcessedOrders { get; } = new();
    public IFireForgetActivator<ICommand> Delegates => HandlerFactory.CreateFireForget<ICommand>(registry => registry.Register<CreateOrderCommand>(command => ProcessedOrders.Add(command.OrderId)));
}

public sealed record CreateOrderCommand(string OrderId) : Request, ICommand;
```
