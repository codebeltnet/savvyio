---
uid: Savvyio.EventDriven.Messaging.CloudEvents.Cryptography
summary: *content
---
Receivers of CloudEvents need cryptographic proof that the event has not been altered in transit. The `Savvyio.EventDriven.Messaging.CloudEvents.Cryptography` namespace provides signing and verification for `ICloudEvent<T>` through `SignedCloudEvent<T>`.

Start with `CloudEventExtensions.SignCloudEvent<T>` to produce a `SignedCloudEvent<T>` with an attached signature. On the consumer side, call `SignedCloudEventExtensions.CheckCloudEventSignature<T>` to verify authenticity before processing. Use this namespace alongside `Savvyio.EventDriven.Messaging.CloudEvents`; for plain message signing without CloudEvents, see `Savvyio.Messaging.Cryptography`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICloudEvent<T>|⬇️|`SignCloudEvent<T>`|
|ISignedCloudEvent<T>|⬇️|`CheckCloudEventSignature<T>`|
