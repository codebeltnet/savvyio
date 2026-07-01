---
uid: Savvyio.Extensions.NATS.Commands
summary: *content
---
Savvy I/O commands delivered via NATS are handled by this namespace. `NatsCommandQueue` implements `ICommandQueue<T>` by publishing serialized commands to a NATS subject and consuming them via a subscription. `NatsCommandQueueOptions` configures the subject, connection, and serialization.

Start with `NatsCommandQueue` as the implementation class for NATS command delivery. Choose this namespace when you need the concrete NATS command-queue type to extend, test, or configure directly; for DI-based registration without touching the implementation type, use `Savvyio.Extensions.DependencyInjection.NATS.AddNatsCommandQueue<TMarker>`. For the corresponding event bus, see `Savvyio.Extensions.NATS.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
