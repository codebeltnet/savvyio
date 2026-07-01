---
uid: Savvyio.Messaging
summary: *content
---
Transport-agnostic messaging in Savvy I/O revolves around `Message<T>`: a typed envelope that pairs a payload with a source URI, a type discriminator, and a unique message ID. The `Savvyio.Messaging` namespace provides this envelope, its options, and the async enumerable types for consuming message streams.

Start with `Message<T>` to create a message envelope around any command or integration event. `MessageOptions` configures the source and type metadata. `MessageAsyncEnumerable<T>` and `MessageAsyncEnumerator<T>` support async pull-based consumption from a queue or bus. Use `IMessage<T>.Clone<T>()` to duplicate a received message before re-processing or republishing it. For signed messages, see `Savvyio.Messaging.Cryptography`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IMessage<T>|⬇️|`Clone<T>`|
