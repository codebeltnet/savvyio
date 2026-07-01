---
uid: Savvyio.Extensions.Newtonsoft.Json.Converters.RequestConverter
example:
- *content
---
Add `RequestConverter` to `JsonSerializerSettings` to rehydrate request types with read-only properties.

```csharp
using Savvyio;
using Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json.Converters;

namespace ExampleApp;

public static class RequestSerialization
{
    public static CreateOrderCommand RoundTrip()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new RequestConverter());

        var json = JsonConvert.SerializeObject(new CreateOrderCommand("SO-42", 2));
        return JsonConvert.DeserializeObject<CreateOrderCommand>(json, settings)!;
    }
}

public sealed record CreateOrderCommand(string OrderId, int Quantity) : Request;
```
