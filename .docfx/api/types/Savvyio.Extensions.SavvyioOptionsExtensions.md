---
uid: Savvyio.Extensions.SavvyioOptionsExtensions
example:
- *content
---
`SavvyioOptionsExtensions` adds `AddMediator<TImplementation>` and the discovery switches `UseAutomaticDispatcherDiscovery` and `UseAutomaticHandlerDiscovery` to `SavvyioOptions`. Chain them to configure the framework for automatic assembly scanning.

```csharp
using System;
using Savvyio;
using Savvyio.Extensions;

namespace ExampleApp;

public sealed class SavvyioOptionsConfiguration
{
    public void Configure()
    {
        var options = new SavvyioOptions()
            .AddMediator<Mediator>()
            .UseAutomaticDispatcherDiscovery()
            .UseAutomaticHandlerDiscovery();

        Console.WriteLine($"HandlerDiscovery: {options.AllowHandlerDiscovery}, DispatcherDiscovery: {options.AllowDispatcherDiscovery}");
    }
}
```

