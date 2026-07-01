---
uid: Savvyio.Messaging.SubscribeAsyncOptions
example:
- *content
---
This example shows how to configure subscription behavior so a cancelled receive loop surfaces an OperationCanceledException when needed.

```csharp
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class SubscribeAsyncOptionsExample
{
    public SubscribeAsyncOptions Configure()
    {
        return new SubscribeAsyncOptions { ThrowIfCancellationWasRequested = true };
    }
}
```
