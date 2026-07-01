---
uid: Savvyio.HandlerServiceTypeImplementationDelegatesModel
example:
- *content
---
`HandlerServiceTypeImplementationDelegatesModel` represents one delegate entry in handler discovery output — the request type handled and the name of the registered method delegate.

```csharp
using System;
using Savvyio;

namespace ExampleApp;

public sealed class HandlerDelegateInspection
{
    public static void PrintDelegates()
    {
        var entry = new HandlerServiceTypeImplementationDelegatesModel(
            typeof(CreateOrderCommand).Name,
            "HandleCreateOrderAsync");

        Console.WriteLine($"Request: {entry.Type}, Handler method: {entry.Handler}");
    }
}

public sealed record CreateOrderCommand(string OrderId);
```

