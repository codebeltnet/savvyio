---
uid: Savvyio.HandlerServiceTypeImplementationModel
example:
- *content
---
This example shows how to read implementation entries from the discovery report a handler descriptor produces.

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Runtime;
using Savvyio;
using Savvyio.Commands;

namespace ExampleApp;

public sealed class HandlerServiceTypeImplementationModelExample
{
    public IEnumerable<string> ReadImplementationNames()
    {
        var descriptor = new HandlerServicesDescriptor(new[] { new Grouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>(typeof(ICommandHandler), new List<KeyValuePair<Type, List<IHierarchy<object>>>>()) }, new[] { typeof(ICommandHandler) });
        IEnumerable<HandlerServiceTypeImplementationModel> implementations = descriptor.GenerateHandlerDiscoveries().SelectMany(model => model.Assemblies ?? Array.Empty<HandlerServiceAssemblyModel>()).SelectMany(assembly => assembly.Implementations ?? Array.Empty<HandlerServiceTypeImplementationModel>());
        return implementations.Select(implementation => implementation.Name);
    }
}

internal sealed class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
{
    private readonly IEnumerable<TElement> _elements;
    public Grouping(TKey key, IEnumerable<TElement> elements) { Key = key; _elements = elements; }
    public TKey Key { get; }
    public IEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```
