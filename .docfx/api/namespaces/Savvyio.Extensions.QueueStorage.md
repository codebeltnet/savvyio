---
uid: Savvyio.Extensions.QueueStorage
summary: *content
---
Azure Queue Storage support in Savvy I/O is organized around the generic `AzureQueue<T>` base class. The `Savvyio.Extensions.QueueStorage` namespace provides that base class, the shared `AzureQueueOptions`, and the send/receive options for fine-grained control.

Start with `AzureQueueOptions` to configure the storage account connection string and queue name. Choose this namespace when you need to extend or customize the Azure Queue Storage base classes directly; for DI-based registration without dealing with base classes, use `Savvyio.Extensions.DependencyInjection.QueueStorage` instead. The concrete command queue is in `Savvyio.Extensions.QueueStorage.Commands` and the event bus in `Savvyio.Extensions.QueueStorage.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
