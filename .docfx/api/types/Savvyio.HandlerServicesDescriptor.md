---
uid: Savvyio.HandlerServicesDescriptor
example:
- *content
---
`HandlerServicesDescriptor` produces a human-readable handler discovery report. Create it with the service groups and types from the DI container or empty collections for diagnostics, then call `ToString()` to get the formatted report.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Savvyio;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class HandlerServicesDescriptorExample
{
    public static void PrintReport()
    {
        var emptyGroups = Enumerable.Empty<IGrouping<Type, KeyValuePair<Type, List<Cuemon.Extensions.Runtime.IHierarchy<object>>>>>();
        var descriptor = new HandlerServicesDescriptor(emptyGroups, new[] { typeof(IHandler<IRequest>) });
        Console.WriteLine(descriptor.ToString());
    }
}
```


