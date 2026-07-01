---
uid: Savvyio.Extensions.Text.Json.Converters.SingleValueObjectConverter
example:
- *content
---
`SingleValueObjectConverter` supports `ISingleValueObject<T>` serialization, writing the underlying primitive value as a JSON value rather than a nested object. This allows value objects like `EmailAddress` or `Money` to round-trip as plain JSON strings or numbers. The example serializes a typed value object and verifies the JSON representation is the raw value.

```csharp
using System.Text.Json;
using Savvyio.Domain;
using Savvyio.Extensions.Text.Json.Converters;

namespace ExampleApp;

public static class SingleValueObjectSerialization
{
    public static OrderNumber RoundTrip(OrderNumber orderNumber)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new SingleValueObjectConverter());

        var json = JsonSerializer.Serialize(orderNumber, options);
        return JsonSerializer.Deserialize<OrderNumber>(json, options)!;
    }
}

public sealed record OrderNumber : SingleValueObject<string>
{
    public OrderNumber(string value) : base(value)
    {
    }
}
```
