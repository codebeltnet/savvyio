---
uid: Savvyio.EventDriven.Messaging.CloudEvents
summary: *content
---
[CloudEvents](https://cloudevents.io/) is a CNCF standard for describing event data in a portable way. The `Savvyio.EventDriven.Messaging.CloudEvents` namespace adapts `IMessage<T>` envelopes to this format, enabling interoperability with CloudEvents-aware brokers and consumers.

Start with `MessageExtensions.ToCloudEvent<T>` to convert an `IMessage<T>` into a `CloudEvent<T>`. Use this namespace when the receiving service expects the CloudEvents schema rather than the native Savvy I/O message format. To add a cryptographic signature to the cloud event, see `Savvyio.EventDriven.Messaging.CloudEvents.Cryptography`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IMessage<T>|⬇️|`ToCloudEvent<T>`|
