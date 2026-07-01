---
uid: Savvyio.Extensions.Text.Json.Converters.RequestConverter
example:
- *content
---
`RequestConverter` is the System.Text.Json converter for `IRequest` implementations, enabling polymorphic deserialization of command and query payloads without explicit type discriminators. Add it to `JsonSerializerOptions.Converters` whenever commands or queries are serialized independently of a message envelope. The example serializes a command and deserializes it back through the interface.

```csharp
using Savvyio;
using System.Text.Json;
using Savvyio.Extensions.Text.Json.Converters;

namespace ExampleApp;

public static class RequestSerialization
{
    public static CreateOrderCommand RoundTrip()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new RequestConverter());

        var json = JsonSerializer.Serialize(new CreateOrderCommand("SO-42", 2), options);
        return JsonSerializer.Deserialize<CreateOrderCommand>(json, options)!;
    }
}

public sealed record CreateOrderCommand(string OrderId, int Quantity) : Request;
```
