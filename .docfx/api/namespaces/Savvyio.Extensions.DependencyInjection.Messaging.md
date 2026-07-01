---
uid: Savvyio.Extensions.DependencyInjection.Messaging
summary: *content
---
Messaging infrastructure belongs behind an abstraction. The `Savvyio.Extensions.DependencyInjection.Messaging` namespace provides `AddMessageQueue<TService, TRequest>` and `AddMessageBus<TService, TRequest>` to bind the abstract messaging interfaces to concrete implementations without tying the domain layer to a specific broker technology.

Start with `AddMessageBus<TService, TRequest>` to register an event bus, or `AddMessageQueue<TService, TRequest>` to register a command queue. For broker-specific registration helpers (NATS, RabbitMQ, Azure Queue Storage, Amazon SQS), use the corresponding `Savvyio.Extensions.DependencyInjection.*` namespace.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddMessageQueue<TService, TRequest>`, `AddMessageBus<TService, TRequest>`|
