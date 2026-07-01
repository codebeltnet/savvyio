---
uid: Savvyio.Dispatchers.ServiceLocator
example:
- *content
---
This example shows how to adapt an application service collection to Savvy I/O with a lightweight service locator.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Savvyio.Dispatchers;

namespace ExampleApp;

public sealed class ServiceLocatorExample
{
    public bool CanResolveDispatcher()
    {
        var services = new Dictionary<Type, object>
        {
            [typeof(ICheckoutDispatcher)] = new CheckoutDispatcher()
        };

        var locator = new ServiceLocator(serviceType =>
            services.TryGetValue(serviceType, out var service)
                ? new[] { service }
                : Array.Empty<object>());

        return locator.GetServices(typeof(ICheckoutDispatcher)).Single() is CheckoutDispatcher;
    }
}

public interface ICheckoutDispatcher
{
}

public sealed class CheckoutDispatcher : ICheckoutDispatcher
{
}
```
