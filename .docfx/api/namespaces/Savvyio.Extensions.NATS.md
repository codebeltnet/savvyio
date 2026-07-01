---
uid: Savvyio.Extensions.NATS
summary: *content
---
Use the `Savvyio.Extensions.NATS` namespace for the core NATS messaging types. It provides `NatsMessage<T>` as the NATS-specific message envelope and `NatsMessageOptions` for configuring subject, connection, and serialization settings shared by both the command queue and event bus.

These base types are used by `Savvyio.Extensions.NATS.Commands` and `Savvyio.Extensions.NATS.EventDriven`. Registering with the DI container is handled by `Savvyio.Extensions.DependencyInjection.NATS`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
