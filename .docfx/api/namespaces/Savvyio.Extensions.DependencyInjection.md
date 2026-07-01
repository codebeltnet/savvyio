---
uid: Savvyio.Extensions.DependencyInjection
summary: *content
---
At host startup, `AddSavvyIO` creates the registration graph that ties together handlers, dispatchers, marshallers, data sources, and the service locator — all in one fluent call. Use this namespace in every Savvy I/O application that runs under Microsoft's DI container.

`AddHandlerServicesDescriptor` enables handler auto-discovery; `AddDataSource<TService>` registers your data access layer; `AddMarshaller<TService>` sets the serialization strategy; `AddServiceLocator` exposes `IServiceLocator`. After startup, call `IServiceProvider.WriteHandlerDiscoveriesToLog<TCategoryName>` to audit which handlers were resolved. Start with `AddSavvyIO` and compose the sub-namespaces — `Savvyio.Extensions.DependencyInjection.EFCore`, `Savvyio.Extensions.DependencyInjection.NATS`, and their siblings — to add broker and persistence-specific registrations.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddSavvyIO`, `AddConfiguredOptions<TOptions>`, `AddMarshaller<TService>`, `AddDataSource<TService>`, `AddServiceLocator`, `AddHandlerServicesDescriptor`|
|IServiceProvider|⬇️|`WriteHandlerDiscoveriesToLog<TCategoryName>`|
