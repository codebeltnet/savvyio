---
uid: Savvyio.Messaging.MessageOptions
example:
- *content
---
This example shows how to configure message envelope metadata before a request is promoted to an outbound message.

```csharp
using System;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class MessageOptionsExample
{
    public MessageOptions Configure()
    {
        var options = new MessageOptions { MessageId = "msg-42", Time = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc) };
        options.ValidateOptions();
        return options;
    }
}
```
