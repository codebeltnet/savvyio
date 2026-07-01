---
uid: Savvyio.Extensions.Newtonsoft.Json.Converters.MessageConverter
example:
- *content
---
Add `MessageConverter` to `JsonSerializerSettings` to serialize and deserialize Savvy I/O message envelopes.

```csharp
using Savvyio;
using System;
using Newtonsoft.Json;
using Savvyio.Extensions.Newtonsoft.Json.Converters;
using Savvyio.Messaging;

namespace ExampleApp;

public static class MessageSerialization
{
    public static Message<ShipOrderCommand> RoundTrip()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new MessageConverter());

        var message = new Message<ShipOrderCommand>(
            "msg-001",
            new Uri("urn:orders"),
            nameof(ShipOrderCommand),
            new ShipOrderCommand("SO-42"),
            DateTime.UtcNow);

        var json = JsonConvert.SerializeObject(message, settings);
        return JsonConvert.DeserializeObject<Message<ShipOrderCommand>>(json, settings)!;
    }
}

public sealed record ShipOrderCommand(string OrderId) : Request;
```
