---
uid: Savvyio.Domain
summary: *content
---
Use the `Savvyio.Domain` namespace to model the core business domain using Domain-Driven Design: aggregate roots with encapsulated domain events, value objects with structural equality, entities with identity, and single-value objects for type-safe primitives.

Start with `AggregateRoot` when your domain concept has a lifecycle and raises events. Use `Entity` for objects with identity that are governed by an aggregate, `ValueObject` for immutable equality by value, and `SingleValueObject` for primitives like identifiers or money amounts. Register a `DomainEventHandler` and configure dispatching with `SavvyioOptions.AddDomainEventDispatcher` and `AddDomainEventHandler`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`GetEventId<T>`, `GetTimestamp<T>`|
|IDomainEventDispatcher|⬇️|`RaiseMany<T>`, `RaiseManyAsync<T>`|
|SavvyioOptions|⬇️|`AddDomainEventHandler<TImplementation>`, `AddDomainEventDispatcher`|
