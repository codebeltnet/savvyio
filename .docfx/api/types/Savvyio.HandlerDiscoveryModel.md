---
uid: Savvyio.HandlerDiscoveryModel
example:
- *content
---
This example shows how to create a handler discovery model from the grouped service output that a descriptor consumes.

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Runtime;
using Savvyio;
using Savvyio.Commands;

namespace ExampleApp;

public sealed class HandlerDiscoveryModelExample
{
    public HandlerDiscoveryModel Create()
    {
        var group = new Grouping<Type, KeyValuePair<Type, List<IHierarchy<object>>>>(typeof(ICommandHandler), new List<KeyValuePair<Type, List<IHierarchy<object>>>>());
        return new HandlerDiscoveryModel(typeof(ICommandHandler), typeof(ICommand), group);
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
