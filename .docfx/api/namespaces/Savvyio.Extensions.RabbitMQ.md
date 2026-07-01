---
uid: Savvyio.Extensions.RabbitMQ
summary: *content
---
Use the `Savvyio.Extensions.RabbitMQ` namespace for the core RabbitMQ integration types. It provides `RabbitMqMessage<T>` as the RabbitMQ message envelope and `RabbitMqMessageOptions` for configuring exchange, routing key, connection, and serialization settings shared by both the command queue and event bus.

The concrete command queue is in `Savvyio.Extensions.RabbitMQ.Commands` and the event bus in `Savvyio.Extensions.RabbitMQ.EventDriven`. Register both with `Savvyio.Extensions.DependencyInjection.RabbitMQ`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
