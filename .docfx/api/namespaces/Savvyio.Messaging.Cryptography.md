---
uid: Savvyio.Messaging.Cryptography
summary: *content
---
Messages traveling through a broker can be tampered with. The `Savvyio.Messaging.Cryptography` namespace protects command and event message integrity by attaching cryptographic signatures through `SignedMessage<T>`, regardless of transport.

Start with `MessageExtensions.Sign<T>` to add an HMAC or asymmetric signature to any `IMessage<T>`, producing a `SignedMessage<T>`. On the consumer side, call `SignedMessageExtensions.CheckSignature<T>` to verify authenticity before dispatching. Choose this namespace when messages travel through untrusted infrastructure and you need proof that the payload has not been altered; for cloud-native message signing without cryptographic overhead, broker-level security policies may be sufficient. For CloudEvents-specific signing, see `Savvyio.EventDriven.Messaging.CloudEvents.Cryptography`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IMessage<T>|⬇️|`Sign<T>`|
|ISignedMessage<T>|⬇️|`CheckSignature<T>`|
