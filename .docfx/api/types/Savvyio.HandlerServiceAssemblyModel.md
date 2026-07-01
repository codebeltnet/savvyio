---
uid: Savvyio.HandlerServiceAssemblyModel
example:
- *content
---
`HandlerServiceAssemblyModel` exposes the assembly name and namespace of a discovered handler implementation, populated during the handler discovery phase at startup. Create one by passing the handler implementation type to the constructor. The example creates a model from a concrete handler type and prints the assembly name and namespace.

```csharp
using System;
using Savvyio;
using Savvyio.Commands;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class HandlerServiceAssemblyModelExample
{
    public void Describe()
    {
        var model = new HandlerServiceAssemblyModel(typeof(CreateOrderHandler));
        Console.WriteLine($"Assembly: {model.Name}, Namespace: {model.Namespace}");
    }
}

public sealed class CreateOrderHandler : ICommandHandler
{
    public IFireForgetActivator<ICommand> Delegates =>
        HandlerFactory.CreateFireForget<ICommand>(r => r.Register<CreateOrderCommand>(_ => { }));
}

public sealed record CreateOrderCommand(string OrderId) : Request, ICommand;
```

