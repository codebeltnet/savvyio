---
uid: Savvyio.Extensions.QueueStorage.EventDriven
summary: *content
---
Savvy I/O integration events delivered via Azure Queue Storage are handled by this namespace. `AzureEventBus` implements `IEventBus<T>` by serializing integration events and enqueuing/dequeuing them via Azure Storage. `AzureEventBusOptions` configures the queue name and serialization.

Start with `AzureEventBus` as the implementation class for Azure Queue Storage event delivery. Choose this namespace when you need the concrete event-bus type to extend, test, or configure directly; for DI-based registration, use `Savvyio.Extensions.DependencyInjection.QueueStorage.AddAzureEventBus<TMarker>`. For the corresponding command queue, see `Savvyio.Extensions.QueueStorage.Commands`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
