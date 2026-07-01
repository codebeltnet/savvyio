---
uid: Savvyio.Extensions.Text.Json.JsonConverterExtensions
example:
- *content
---
`JsonConverterExtensions` provides fluent methods for adding the complete set of Savvy I/O converters to `JsonSerializerOptions.Converters`. The prerequisite is an existing `JsonSerializerOptions` instance; each extension method returns the same `ICollection<JsonConverter>` so calls can be chained. The example chains all available registration methods and serializes an `IMessage<T>` to verify the converters compose correctly.

```csharp
using Savvyio;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Savvyio.Extensions.Text.Json;
using Savvyio.Messaging;

namespace ExampleApp;

public static class JsonOptionsFactory
{
    public static JsonSerializerOptions Create()
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        options.Converters.RemoveAllOf(typeof(JsonConverter));
        options.Converters.RemoveAllOf<JsonConverter>();
        options.Converters.AddMetadataDictionaryConverter();
        options.Converters.AddMessageConverter();
        options.Converters.AddRequestConverter();
        options.Converters.AddSingleValueObjectConverter();
        options.Converters.AddDateTimeConverter();
        options.Converters.AddDateTimeOffsetConverter();

        var message = new Message<PlaceOrderCommand>(
            "msg-001",
            new Uri("urn:orders"),
            nameof(PlaceOrderCommand),
            new PlaceOrderCommand("SO-42"),
            DateTime.UtcNow);

        _ = JsonSerializer.Serialize(message, options);
        return options;
    }
}

public sealed record PlaceOrderCommand(string OrderId) : Request;
```

