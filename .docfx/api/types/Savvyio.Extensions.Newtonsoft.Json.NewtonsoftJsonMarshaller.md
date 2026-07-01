---
uid: Savvyio.Extensions.Newtonsoft.Json.NewtonsoftJsonMarshaller
example:
- *content
---
Use `NewtonsoftJsonMarshaller` to serialize and deserialize Savvy I/O requests through streams.

```csharp
using Savvyio;
using Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json;

namespace ExampleApp;

public static class MarshallerExample
{
    public static ShipOrderCommand RoundTrip(ShipOrderCommand command)
    {
        var marshaller = NewtonsoftJsonMarshaller.Create(options =>
        {
            options.Settings.Formatting = Formatting.Indented;
        });

        using var stream = marshaller.Serialize(command);
        stream.Position = 0;
        return marshaller.Deserialize<ShipOrderCommand>(stream);
    }
}

public sealed record ShipOrderCommand(string OrderId) : Request;
```
