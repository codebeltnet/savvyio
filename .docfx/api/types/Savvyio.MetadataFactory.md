---
uid: Savvyio.MetadataFactory
example:
- *content
---
This example shows how to persist a custom metadata value on a request and retrieve it later in the pipeline.

```csharp
using Savvyio;

namespace ExampleApp;

public sealed class MetadataFactoryExample
{
    public string GetTenant()
    {
        var command = new CreateOrderCommand("ORD-42");
        MetadataFactory.Set(command, "tenant", "eu-west");
        return (string)MetadataFactory.Get(command, "tenant");
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```
