---
uid: Savvyio.Extensions.RabbitMQ.EventDriven
summary: *content
---
Savvy I/O integration events published via RabbitMQ are handled by this namespace. `RabbitMqEventBus` implements `IEventBus<T>` by publishing integration events to a RabbitMQ exchange and consuming them from bound queues. `RabbitMqEventBusOptions` configures the exchange, routing key, and connection.

Start with `RabbitMqEventBus` as the implementation class for RabbitMQ event delivery. Choose this namespace when you need the concrete event-bus type to extend, test, or configure directly; for DI-based registration, use `Savvyio.Extensions.DependencyInjection.RabbitMQ.AddRabbitMqEventBus<TMarker>`. For the corresponding command queue, see `Savvyio.Extensions.RabbitMQ.Commands`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
