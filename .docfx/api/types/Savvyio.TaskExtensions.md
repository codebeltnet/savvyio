---
uid: Savvyio.TaskExtensions
example:
- *content
---
This example shows how to await a task that returns a sequence and keep only the single matching result.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio;

namespace ExampleApp;

public sealed class TaskExtensionsExample
{
    public Task<OrderProjection?> LoadAsync()
    {
        Task<IEnumerable<OrderProjection>> query = Task.FromResult<IEnumerable<OrderProjection>>(new[] { new OrderProjection("ORD-42") });
        return query.SingleOrDefaultAsync();
    }
}

public sealed record OrderProjection(string OrderId);
```
