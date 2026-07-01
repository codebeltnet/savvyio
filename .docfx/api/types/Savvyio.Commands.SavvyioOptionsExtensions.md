---
uid: Savvyio.Commands.SavvyioOptionsExtensions
example:
- *content
---
This example shows how to configure SavvyioOptions for a command-only application service. The options register both the command handler implementation and the default command dispatcher so command commits can resolve without manual type bookkeeping.

```csharp
using Savvyio;
using Savvyio.Commands;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class CommandOptionsExtensionsExample
{
    public int Configure()
    {
        var options = new SavvyioOptions().AddCommandHandler<CreateOrderHandler>().AddCommandDispatcher();
        return options.HandlerImplementationTypes.Count + options.DispatcherImplementationTypes.Count;
    }
}

public sealed class CreateOrderHandler : ICommandHandler
{
    public IFireForgetActivator<ICommand> Delegates => HandlerFactory.CreateFireForget<ICommand>(registry => registry.Register<CreateOrderCommand>(_ => { }));
}

public sealed record CreateOrderCommand(string OrderId) : Request, ICommand;
```
