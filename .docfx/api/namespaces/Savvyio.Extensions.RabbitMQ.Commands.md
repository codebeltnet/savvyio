---
uid: Savvyio.Extensions.RabbitMQ.Commands
summary: *content
---
Savvy I/O commands delivered via RabbitMQ are handled by this namespace. `RabbitMqCommandQueue` implements `ICommandQueue<T>` by publishing serialized commands to a RabbitMQ exchange and consuming them from a bound queue. `RabbitMqCommandQueueOptions` configures the exchange, routing key, and connection.

Start with `RabbitMqCommandQueue` as the implementation class for RabbitMQ command delivery. Choose this namespace when you need the concrete command-queue type to extend, test, or configure directly; for DI-based registration, use `Savvyio.Extensions.DependencyInjection.RabbitMQ.AddRabbitMqCommandQueue<TMarker>`. For the corresponding event bus, see `Savvyio.Extensions.RabbitMQ.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
