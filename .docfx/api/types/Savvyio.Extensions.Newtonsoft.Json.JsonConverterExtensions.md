---
uid: Savvyio.Extensions.Newtonsoft.Json.JsonConverterExtensions
example:
- *content
---
Use `JsonConverterExtensions` to add all Savvy I/O-aware converters to a Newtonsoft.Json serializer configuration.

```csharp
using Savvyio;
using System;
using Newtonsoft.Json;
using Savvyio.Commands;
using Savvyio.Domain;
using Savvyio.Extensions.Newtonsoft.Json;
using Savvyio.Messaging;

namespace ExampleApp;

public static class JsonSettingsFactory
{
    public static JsonSerializerSettings Create()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters
            .AddMetadataDictionaryConverter()
            .AddMessageConverter()
            .AddRequestConverter()
            .AddSingleValueObjectConverter()
            .AddValueObjectConverter()
            .AddAggregateRootConverter<Guid>();

        var message = new Message<PublishOrderCommand>(
            "msg-001",
            new Uri("urn:orders"),
            nameof(PublishOrderCommand),
            new PublishOrderCommand("SO-42"),
            DateTime.UtcNow);

        _ = JsonConvert.SerializeObject(message, settings);
        return settings;
    }
}

public sealed record PublishOrderCommand(string OrderId) : Request;
```

