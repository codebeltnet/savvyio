---
uid: Savvyio.EventDriven
summary: *content
---
Use the `Savvyio.EventDriven` namespace to define and dispatch integration events — messages that cross service boundaries and enable eventual consistency in a distributed system. An integration event announces that something has happened in one service so that other services can react.

Start with `IntegrationEvent` as the base class for your cross-service event payloads. Register an `IntegrationEventHandler` and connect it to the DI container with `SavvyioOptions.AddIntegrationEventDispatcher` and `AddIntegrationEventHandler`. Use the extension methods on `IIntegrationEvent` to read the event ID, timestamp, and member type from the event's metadata.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`GetEventId<T>`, `GetTimestamp<T>`, `GetMemberType<T>`|
|SavvyioOptions|⬇️|`AddIntegrationEventHandler<TImplementation>`, `AddIntegrationEventDispatcher`|
