---
uid: Savvyio.Extensions.DependencyInjection.NATS
summary: *content
---
NATS messaging for Savvy I/O is registered in one call per channel. The `Savvyio.Extensions.DependencyInjection.NATS` namespace provides `AddNatsCommandQueue<TMarker>` and `AddNatsEventBus<TMarker>` to configure the NATS connection and register the corresponding service.

Start with `AddNatsCommandQueue<TMarker>` to register a NATS command queue, and `AddNatsEventBus<TMarker>` for an event bus. Choose this namespace when your application uses NATS as the message broker for low-latency command and event delivery; for RabbitMQ, Azure Queue Storage, or Amazon SQS, see the corresponding `Savvyio.Extensions.DependencyInjection.*` namespace. The concrete implementation types and their options are in `Savvyio.Extensions.DependencyInjection.NATS.Commands` and `Savvyio.Extensions.DependencyInjection.NATS.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddNatsCommandQueue`, `AddNatsCommandQueue<TMarker>`, `AddNatsEventBus`, `AddNatsEventBus<TMarker>`|
