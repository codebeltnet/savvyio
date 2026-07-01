---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.EventDriven
summary: *content
---
The concrete RabbitMQ event-bus type and its options used during DI registration live in this namespace. `RabbitMqEventBus` publishes and receives integration events via RabbitMQ exchanges. `RabbitMqEventBusOptions` carries the exchange, routing key, and connection settings.

Start with `RabbitMqEventBus` when you need to inspect or extend the registered implementation. Configure its options through `Savvyio.Extensions.DependencyInjection.RabbitMQ.AddRabbitMqEventBus<TMarker>` rather than instantiating it directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
