---
uid: Savvyio.Domain.EventSourcing
summary: *content
---
Instead of storing current aggregate state, event sourcing records every state-changing event and reconstructs the aggregate by replaying them. The `Savvyio.Domain.EventSourcing` namespace provides the base types that make this possible.

Start with `TracedAggregateRoot` as the base class for aggregates whose history must be persisted. Each state change produces a `TracedDomainEvent` that carries the aggregate ID, version, member type, and the delta. Use the extension methods on `ITracedDomainEvent` to read and write aggregate version metadata. Persistence is provided by `Savvyio.Extensions.EFCore.Domain.EventSourcing`; DI registration is in `Savvyio.Extensions.DependencyInjection.Domain.EventSourcing`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`SetAggregateVersion<T>`, `GetAggregateVersion<T>`, `GetMemberType<T>`|
