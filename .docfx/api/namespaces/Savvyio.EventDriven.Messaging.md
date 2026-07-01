---
uid: Savvyio.EventDriven.Messaging
summary: *content
---
Publishers wrap an integration event in an `IMessage<T>` envelope using `IntegrationEventExtensions.ToMessage<T>` before routing it to a broker. Subscribers unwrap the payload and dispatch the domain event. Start with `IntegrationEventExtensions.ToMessage<T>` to produce the envelope from any `IIntegrationEvent`.

Pass the `IMessage<T>` to an event bus from one of the transport extension packages (NATS, RabbitMQ, Azure Queue Storage, Amazon SNS/SQS). `InMemoryEventBus` stands in during unit tests for any broker. Brokerless in-process dispatch goes through `IntegrationEventDispatcher` from `Savvyio.EventDriven`, which does not use a message envelope.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`ToMessage<T>`|
