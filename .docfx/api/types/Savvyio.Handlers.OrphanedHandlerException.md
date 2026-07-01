---
uid: Savvyio.Handlers.OrphanedHandlerException
example:
- *content
---
Throw an `OrphanedHandlerException` when a dispatcher receives a request type with no registered handler. Use `OrphanedHandlerException.Create<TRequest, THandler>` to produce the exception with a formatted message.

```csharp
using System;
using Savvyio;
using Savvyio.Dispatchers;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class DispatcherWithValidation
{
    public void ValidateHandlerPresence<THandler>(IRequest request)
        where THandler : IHandler<IRequest>
    {
        try
        {
            throw OrphanedHandlerException.Create<IRequest, THandler>(request, "request");
        }
        catch (OrphanedHandlerException exception)
        {
            Console.WriteLine($"No handler found: {exception.Message}");
        }
    }
}

public sealed record PlaceOrderRequest(string OrderId) : Request;
```


