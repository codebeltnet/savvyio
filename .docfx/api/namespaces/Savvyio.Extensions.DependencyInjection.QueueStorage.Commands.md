---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.Commands
summary: *content
---
The concrete Azure Queue Storage command-queue type used during DI registration lives in this namespace. `AzureCommandQueue` enqueues serialized commands to an Azure Storage queue.

Start with `AzureCommandQueue` when you need to inspect or extend the registered implementation type. Configure it through `Savvyio.Extensions.DependencyInjection.QueueStorage.AddAzureCommandQueue<TMarker>` rather than instantiating it directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
