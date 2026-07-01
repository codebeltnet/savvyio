---
uid: Savvyio.Extensions
summary: *content
---
Use the `Savvyio.Extensions` namespace to add a `Mediator` as the single unified entry point for commands, queries, and integration events in a CQRS application. Instead of injecting a separate dispatcher for each request type, inject `IMediator` and dispatch everything through one interface.

Start with `SavvyioOptions.AddMediator` to register `Mediator` during DI setup. Use `UseAutomaticDispatcherDiscovery` and `UseAutomaticHandlerDiscovery` when you want the framework to scan assemblies and wire up dispatchers and handlers without manual registration.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|SavvyioOptions|⬇️|`AddMediator<TImplementation>`, `UseAutomaticDispatcherDiscovery`, `UseAutomaticHandlerDiscovery`|
