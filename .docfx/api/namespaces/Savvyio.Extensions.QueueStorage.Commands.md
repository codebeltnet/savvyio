---
uid: Savvyio.Extensions.QueueStorage.Commands
summary: *content
---
Use the `Savvyio.Extensions.QueueStorage.Commands` namespace to send and receive commands through Azure Queue Storage. `AzureCommandQueue` implements `ICommandQueue<T>` by serializing commands and enqueuing them to an Azure Storage queue, then dequeuing and deserializing them on the consumer side.

Register with `Savvyio.Extensions.DependencyInjection.QueueStorage.AddAzureCommandQueue`. For the corresponding event bus, see `Savvyio.Extensions.QueueStorage.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
