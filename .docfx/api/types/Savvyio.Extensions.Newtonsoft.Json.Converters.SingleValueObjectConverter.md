---
uid: Savvyio.Extensions.Newtonsoft.Json.Converters.SingleValueObjectConverter
example:
- *content
---
Add `SingleValueObjectConverter` to `JsonSerializerSettings` to serialize a single-value object as its wrapped value.

```csharp
using Newtonsoft.Json;
using Savvyio.Domain;
using Savvyio.Extensions.Newtonsoft.Json.Converters;

namespace ExampleApp;

public static class SingleValueObjectSerialization
{
    public static OrderNumber RoundTrip(OrderNumber orderNumber)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new SingleValueObjectConverter());

        var json = JsonConvert.SerializeObject(orderNumber, settings);
        return JsonConvert.DeserializeObject<OrderNumber>(json, settings)!;
    }
}

public sealed record OrderNumber : SingleValueObject<string>
{
    public OrderNumber(string value) : base(value)
    {
    }
}
```
