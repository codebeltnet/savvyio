---
uid: Savvyio.Extensions.DapperExtensions.DapperExtensionsQueryOptions`1
example:
- *content
---
This example shows how `DapperExtensionsQueryOptions<T>` expresses a field predicate that a `DapperExtensionsDataStore<T>` can apply.

```csharp
using DapperExtensions;
using DapperExtensions.Predicate;
using Savvyio.Extensions.DapperExtensions;

namespace ExampleApp;

public sealed class PredicateOptionsExample
{
    public DapperExtensionsQueryOptions<OrderProjection> Create()
    {
        return new DapperExtensionsQueryOptions<OrderProjection>
        {
            Predicate = order => order.Status,
            Op = Operator.Eq,
            Value = "Pending"
        };
    }
}

public sealed class OrderProjection
{
    public long Id { get; set; }

    public string Status { get; set; } = string.Empty;
}
```
