---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage
summary: *content
---
Azure Queue Storage messaging for Savvy I/O is registered in one call per channel. The `Savvyio.Extensions.DependencyInjection.QueueStorage` namespace provides `AddAzureCommandQueue<TMarker>` and `AddAzureEventBus<TMarker>` to configure the Azure Storage connection and register the corresponding service.

Start with `AddAzureCommandQueue<TMarker>` to register an Azure command queue, and `AddAzureEventBus<TMarker>` for an event bus. Choose this namespace when your application targets Azure and you want cost-effective, serverless message delivery through Azure Queue Storage; for higher-throughput or broker-based messaging, consider `Savvyio.Extensions.DependencyInjection.RabbitMQ` or `Savvyio.Extensions.DependencyInjection.NATS`. The concrete types are in `Savvyio.Extensions.DependencyInjection.QueueStorage.Commands` and `Savvyio.Extensions.DependencyInjection.QueueStorage.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddAzureCommandQueue`, `AddAzureCommandQueue<TMarker>`, `AddAzureEventBus`, `AddAzureEventBus<TMarker>`|
