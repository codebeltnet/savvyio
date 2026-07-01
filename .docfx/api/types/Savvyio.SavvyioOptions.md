---
uid: Savvyio.SavvyioOptions
example:
- *content
---
Configure `SavvyioOptions` to enable handler and dispatcher discovery, add specific handler and dispatcher registrations, and inspect the resulting counts.

```csharp
using System;
using Savvyio;
using Savvyio.Dispatchers;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class SavvyioOptionsExample
{
    public void Configure()
    {
        var options = new SavvyioOptions()
            .EnableHandlerDiscovery()
            .EnableDispatcherDiscovery()
            .EnableHandlerServicesDescriptor()
            .AddHandler<IFireForgetHandler<IRequest>, IRequest, CreateOrderHandler>()
            .AddDispatcher<IDispatcher, FireForgetDispatcher>();

        Console.WriteLine($"Handlers: {options.HandlerImplementationTypes.Count}, Dispatchers: {options.DispatcherImplementationTypes.Count}");
        Console.WriteLine($"HandlerDiscovery: {options.AllowHandlerDiscovery}, DispatcherDiscovery: {options.AllowDispatcherDiscovery}");
    }
}

public sealed class CreateOrderHandler : IFireForgetHandler<IRequest>
{
    public IFireForgetActivator<IRequest> Delegates =>
        HandlerFactory.CreateFireForget<IRequest>(r => r.Register<CreateOrderCommand>(_ => { }));
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```


