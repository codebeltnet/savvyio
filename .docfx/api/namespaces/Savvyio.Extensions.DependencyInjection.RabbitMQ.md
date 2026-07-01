---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ
summary: *content
---
RabbitMQ messaging for Savvy I/O is registered in one call per channel. The `Savvyio.Extensions.DependencyInjection.RabbitMQ` namespace provides `AddRabbitMqCommandQueue<TMarker>` and `AddRabbitMqEventBus<TMarker>` to configure the RabbitMQ connection, exchange, and queue settings and register the corresponding service.

Start with `AddRabbitMqCommandQueue<TMarker>` to register a RabbitMQ command queue, and `AddRabbitMqEventBus<TMarker>` for an event bus. Choose this namespace when your application uses RabbitMQ as the broker for durable, broker-managed message delivery with flexible exchange routing; for lighter-weight or cloud-native alternatives, see `Savvyio.Extensions.DependencyInjection.NATS`, `Savvyio.Extensions.DependencyInjection.QueueStorage`, or `Savvyio.Extensions.DependencyInjection.SimpleQueueService`. The concrete types are in `Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands` and `Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddRabbitMqCommandQueue`, `AddRabbitMqCommandQueue<TMarker>`, `AddRabbitMqEventBus`, `AddRabbitMqEventBus<TMarker>`|
