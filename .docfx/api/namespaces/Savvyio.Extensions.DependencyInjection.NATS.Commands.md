---
uid: Savvyio.Extensions.DependencyInjection.NATS.Commands
summary: *content
---
The concrete NATS command-queue types used during DI registration live in this namespace. `NatsCommandQueue` is the registered implementation and `NatsCommandQueueOptions` carries the NATS subject, connection, and serialization settings.

Start with `NatsCommandQueue` when you need to inspect or extend the registered implementation type. Configure its options through `Savvyio.Extensions.DependencyInjection.NATS.AddNatsCommandQueue<TMarker>` rather than instantiating these types directly.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
