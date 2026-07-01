---
uid: Savvyio.Extensions.Newtonsoft.Json.Converters.AggregateRootConverter`1
example:
- *content
---
Add `AggregateRootConverter<TKey>` to `JsonSerializerSettings` to round-trip aggregate roots through Newtonsoft.Json.

```csharp
using System;
using Newtonsoft.Json;
using Savvyio.Domain;
using Savvyio.Extensions.Newtonsoft.Json.Converters;

namespace ExampleApp;

public static class AggregateRootSerialization
{
    public static OrderAggregate RoundTrip(OrderAggregate order)
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new AggregateRootConverter<Guid>());

        var json = JsonConvert.SerializeObject(order, settings);
        return JsonConvert.DeserializeObject<OrderAggregate>(json, settings)!;
    }
}

public sealed class OrderAggregate : AggregateRoot<Guid>
{
    public OrderAggregate(Guid id, string customerId) : base(id)
    {
        CustomerId = customerId;
    }

    public string CustomerId { get; }
}
```
