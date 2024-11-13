---
uid: Savvyio
summary: *content
---
The `Savvyio` namespace provides the fundamental abstractions and classes for supporting a complete flow of DDD, CQRS and Event Sourcing concepts including the option to scale out using distributed subsystems.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Methods

|Type|Ext|Methods|
|--:|:-:|---|
|IMetadata|⬇️|`GetCausationId`, `GetCorrelationId`, `GetMemberType`, `SetCausationId`, `SetCorrelationId`, `SetEventId`, `SetTimestamp`, `SetMemberType`, `SaveMetadata`, `MergeMetadata`|
|SavvyioOptions|⬇️|`AddDispatchers`, `AddHandlers`|
|Task|⬇️|`SingleOrDefaultAsync`|
