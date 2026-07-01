---
uid: Savvyio.Messaging.AcknowledgedEventArgs
example:
- *content
---
This example shows how to capture the AcknowledgedEventArgs payload that a message publishes after successful processing. The handler promotes transport properties into the event args so later pipeline steps can inspect the acknowledgement state.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class AcknowledgedEventArgsExample
{
    public async Task<IDictionary<string, object>> CaptureAsync()
    {
        var message = new Message<CreateOrderCommand>("msg-42", new Uri("urn:orders"), "orders.created", new CreateOrderCommand("ORD-42"));
        message.Properties["tenant"] = "eu-west";
        AcknowledgedEventArgs? observed = null;
        message.Acknowledged += (_, args) =>
        {
            observed = args;
            return Task.CompletedTask;
        };
        await message.AcknowledgeAsync().ConfigureAwait(false);
        return observed?.Properties ?? new Dictionary<string, object>();
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;
```
