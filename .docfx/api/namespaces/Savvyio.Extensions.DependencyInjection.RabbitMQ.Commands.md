---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.Commands
summary: *content
---
The concrete RabbitMQ command-queue type and its options used during DI registration live in this namespace. `RabbitMqCommandQueue` sends serialized commands to a RabbitMQ exchange. `RabbitMqCommandQueueOptions` carries the exchange, routing key, and connection settings.

Start with `RabbitMqCommandQueue` when you need to inspect or extend the registered implementation. Configure its options through `Savvyio.Extensions.DependencyInjection.RabbitMQ.AddRabbitMqCommandQueue<TMarker>` rather than instantiating it directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
