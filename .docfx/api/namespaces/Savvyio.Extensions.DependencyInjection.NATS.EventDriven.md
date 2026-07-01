---
uid: Savvyio.Extensions.DependencyInjection.NATS.EventDriven
summary: *content
---
The concrete NATS event-bus types used during DI registration live in this namespace. `NatsEventBus` is the registered implementation and `NatsEventBusOptions` carries the NATS subject, connection, and serialization settings.

Start with `NatsEventBus` when you need to inspect or extend the registered implementation type. Configure its options through `Savvyio.Extensions.DependencyInjection.NATS.AddNatsEventBus<TMarker>` rather than instantiating these types directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
