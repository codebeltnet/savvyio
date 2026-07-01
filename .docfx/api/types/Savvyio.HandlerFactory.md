---
uid: Savvyio.HandlerFactory
example:
- *content
---
`HandlerFactory` creates the activator delegates used internally by Savvy I/O handlers to bind request types to handler methods. Use `HandlerFactory.CreateFireForget<TRequest>` to build fire-and-forget delegates or `CreateRequestReply<TRequest, TResponse>` for delegates that return a response.

```csharp
using System;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class HandlerFactoryExample
{
    public async Task DispatchAsync()
    {
        var notifications = HandlerFactory.CreateFireForget<IRequest>(
            r => r.Register<CreateOrderCommand>(cmd => Console.WriteLine("Processing: " + cmd.OrderId)));

        var command = new CreateOrderCommand("ORD-42");
        notifications.TryInvoke(command);
        await notifications.TryInvokeAsync(command).ConfigureAwait(false);
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```

