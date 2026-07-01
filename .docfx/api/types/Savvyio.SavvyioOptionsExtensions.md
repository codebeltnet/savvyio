---
uid: Savvyio.SavvyioOptionsExtensions
example:
- *content
---
This example shows how to scan an assembly for handler and dispatcher contracts so Savvy I/O can register them automatically.

```csharp
using Savvyio;
using Savvyio.Commands;
using Savvyio.Dispatchers;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class SavvyioOptionsExtensionsExample
{
    public SavvyioOptions Configure() => new SavvyioOptions().AddHandlers(typeof(CreateOrderHandler).Assembly).AddDispatchers(typeof(CheckoutDispatcher).Assembly);
}

public interface ICheckoutDispatcher : IDispatcher { }
public sealed class CheckoutDispatcher : ICheckoutDispatcher { }
public sealed class CreateOrderHandler : ICommandHandler
{
    public IFireForgetActivator<ICommand> Delegates => HandlerFactory.CreateFireForget<ICommand>(registry => registry.Register<CreateOrderCommand>(_ => { }));
}
public sealed record CreateOrderCommand(string OrderId) : Request, ICommand;
```
