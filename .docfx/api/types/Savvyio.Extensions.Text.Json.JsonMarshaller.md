---
uid: Savvyio.Extensions.Text.Json.JsonMarshaller
example:
- *content
---
Use `JsonMarshaller` to serialize and deserialize Savvy I/O requests with System.Text.Json.

```csharp
using Savvyio;
using System.Text.Json;
using Savvyio.Extensions.Text.Json;

namespace ExampleApp;

public static class MarshallerExample
{
    public static ShipOrderCommand RoundTrip(ShipOrderCommand command)
    {
        var marshaller = JsonMarshaller.Create(options =>
        {
            options.Settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Settings.WriteIndented = true;
        });

        using var stream = marshaller.Serialize(command);
        stream.Position = 0;
        return marshaller.Deserialize<ShipOrderCommand>(stream);
    }
}

public sealed record ShipOrderCommand(string OrderId) : Request;
```
