---
uid: Savvyio.Extensions.Newtonsoft.Json.JsonSerializerExtensions
example:
- *content
---
Use `ResolvePropertyKeyByConvention` and `ResolveDictionaryKeyByConvention` to derive JSON property names and dictionary key names from the serializer's naming strategy.

```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Savvyio.Extensions.Newtonsoft.Json;

namespace ExampleApp;

public static class NamingConventions
{
    public static (string PropertyKey, string DictionaryKey) ResolveKeys()
    {
        var serializer = JsonSerializer.Create(new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        });

        var propertyKey = serializer.ResolvePropertyKeyByConvention(nameof(OrderProjection.OrderId));
        var dictionaryKey = serializer.ResolveDictionaryKeyByConvention(nameof(OrderProjection.OrderId));
        return (propertyKey, dictionaryKey);
    }
}

public sealed class OrderProjection
{
    public string OrderId { get; init; } = string.Empty;
}
```

