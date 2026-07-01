---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven
summary: *content
---
The concrete Azure Queue Storage event-bus type and its options used during DI registration live in this namespace. `AzureEventBus` publishes and receives integration events via Azure Queue Storage. `AzureEventBusOptions` carries the queue name and serialization settings.

Start with `AzureEventBus` when you need to inspect or extend the registered implementation type. Configure it through `Savvyio.Extensions.DependencyInjection.QueueStorage.AddAzureEventBus<TMarker>` rather than instantiating it directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
