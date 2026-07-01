---
uid: Savvyio.AsyncEventHandler`1
example:
- *content
---
This example shows how to subscribe an asynchronous event handler that enriches event data before the publisher continues.

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio;

namespace ExampleApp;

public sealed class OrderMessagePump
{
    public event AsyncEventHandler<OrderMessageReceivedEventArgs>? MessageReceived;

    public Task PublishAsync(string orderId) => MessageReceived?.Invoke(this, new OrderMessageReceivedEventArgs(orderId)) ?? Task.CompletedTask;
}

public sealed class OrderMessageReceivedEventArgs : EventArgs
{
    public OrderMessageReceivedEventArgs(string orderId)
    {
        OrderId = orderId;
        Metadata = new Dictionary<string, object>();
    }

    public string OrderId { get; }

    public IDictionary<string, object> Metadata { get; }
}

public sealed class AsyncEventHandlerExample
{
    public async Task<IDictionary<string, object>> CaptureAsync()
    {
        var pump = new OrderMessagePump();
        IDictionary<string, object>? snapshot = null;
        pump.MessageReceived += async (_, args) =>
        {
            args.Metadata["orderId"] = args.OrderId;
            args.Metadata["processedAtUtc"] = DateTime.UtcNow;
            snapshot = args.Metadata;
            await Task.CompletedTask;
        };
        await pump.PublishAsync("ORD-42").ConfigureAwait(false);
        return snapshot ?? new Dictionary<string, object>();
    }
}
```
