---
uid: Savvyio.Extensions.Newtonsoft.Json.Converters.ValueObjectConverter
example:
- *content
---
Add `ValueObjectConverter` to `JsonSerializerSettings` to round-trip richer value objects with multiple properties.

```csharp
using Newtonsoft.Json;
using Savvyio.Domain;
using Savvyio.Extensions.Newtonsoft.Json.Converters;

namespace ExampleApp;

public static class ValueObjectSerialization
{
    public static Money RoundTrip(Money value)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new ValueObjectConverter());

        var json = JsonConvert.SerializeObject(value, settings);
        return JsonConvert.DeserializeObject<Money>(json, settings)!;
    }
}

public sealed record Money : ValueObject
{
    public Money(string currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public string Currency { get; }

    public decimal Amount { get; }
}
```
