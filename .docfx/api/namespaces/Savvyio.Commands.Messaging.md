---
uid: Savvyio.Commands.Messaging
summary: *content
---
Unlike in-process command dispatch through `CommandDispatcher`, commands sent to external services need a transport envelope. `CommandExtensions.ToMessage<T>` wraps any `ICommand` in an `IMessage<T>` envelope, and `InMemoryCommandQueue` replays that envelope in unit tests without a real broker.

Start with `CommandExtensions.ToMessage<T>`, hand the `IMessage<T>` to a queue from one of the transport extension packages (NATS, RabbitMQ, Azure Queue Storage, Amazon SQS), and swap in `InMemoryCommandQueue` when you need to test the dispatch pipeline without infrastructure.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`ToMessage<T>`|
