---
uid: Savvyio.Extensions.Text.Json.Converters.MessageConverter
example:
- *content
---
`MessageConverter` is the System.Text.Json converter that serializes and deserializes `IMessage<T>` envelopes including their source URI, type discriminator, creation time, and typed payload. Add it to `JsonSerializerOptions.Converters` before serializing any message. The example serializes an order command wrapped in `Message<T>` and rounds it back to an `IMessage<CreateOrderCommand>`.

```csharp
using Savvyio;
using System;
using System.Text.Json;
using Savvyio.Extensions.Text.Json.Converters;
using Savvyio.Messaging;

namespace ExampleApp;

public static class MessageSerialization
{
    public static Message<ShipOrderCommand> RoundTrip()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        options.Converters.Add(new MessageConverter());

        var message = new Message<ShipOrderCommand>(
            "msg-001",
            new Uri("urn:orders"),
            nameof(ShipOrderCommand),
            new ShipOrderCommand("SO-42"),
            DateTime.UtcNow);

        var json = JsonSerializer.Serialize(message, options);
        return JsonSerializer.Deserialize<Message<ShipOrderCommand>>(json, options)!;
    }
}

public sealed record ShipOrderCommand(string OrderId) : Request;
```
