---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven.AzureEventBus`1
example:
- *content
---
`AzureEventBus<TMarker>` is the DI-registered Azure Queue Storage event bus with a lifetime marker. It implements `IPublishSubscribeChannel<IIntegrationEvent>`. Inject it to subscribe to and publish integration events through an Azure Storage queue.

```csharp
using System;
using System.Threading.Tasks;
using Savvyio.EventDriven;
using Savvyio.Extensions.QueueStorage.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public sealed class EventListener
{
    public static async Task SubscribeAsync(AzureEventBus bus)
    {
        await bus.SubscribeAsync(async (message, token) =>
        {
            Console.WriteLine($"Received event: {message.Type}");
            await Task.CompletedTask;
        }).ConfigureAwait(false);
    }
}
```
