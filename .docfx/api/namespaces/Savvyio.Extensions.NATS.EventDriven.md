---
uid: Savvyio.Extensions.NATS.EventDriven
summary: *content
---
Savvy I/O integration events published via NATS are handled by this namespace. `NatsEventBus` implements `IEventBus<T>` by publishing serialized integration events to a NATS subject and subscribing to receive them. `NatsEventBusOptions` configures the subject, connection, and serialization.

Start with `NatsEventBus` as the implementation class for NATS event delivery. Choose this namespace when you need the concrete NATS event-bus type to extend, test, or configure directly; for DI-based registration, use `Savvyio.Extensions.DependencyInjection.NATS.AddNatsEventBus<TMarker>`. For the corresponding command queue, see `Savvyio.Extensions.NATS.Commands`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
