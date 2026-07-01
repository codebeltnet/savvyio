---
uid: Savvyio
summary: *content
---
Every Savvy I/O application shares one foundational requirement: a way to stamp causation IDs, correlation IDs, and timestamps onto requests, commands, queries, and events — and a single configuration surface that wires the handler and dispatcher graph into Microsoft DI. This namespace contains both the `IMetadata` contract and the `SavvyioOptions` model, making it the lowest-level dependency for every other Savvy I/O package.

Start with `SavvyioOptions` — configure it through `AddSavvyIO` from `Savvyio.Extensions.DependencyInjection` and extend it with `AddDispatchers`, `AddHandlers`, and the more targeted overloads in the command, domain, query, and event-driven namespaces. Use `IMetadata` and its extension methods to stamp causation IDs, correlation IDs, and timestamps onto any request, command, or event as it flows through the system. This namespace is where you begin whenever you set up a new Savvy I/O application or extend its metadata model.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|T|⬇️|`GetCausationId<T>`, `GetCorrelationId<T>`, `GetRequestId<T>`, `GetMemberType<T>`, `SetCausationId<T>`, `SetCorrelationId<T>`, `SetRequestId<T>`, `SetEventId<T>`, `SetTimestamp<T>`, `SetMemberType<T>`, `SaveMetadata<T>`|
|TDestination|⬇️|`MergeMetadata<TSource, TDestination>`|
|SavvyioOptions|⬇️|`AddDispatchers`, `AddHandlers`|
|Task<IEnumerable<T>>|⬇️|`SingleOrDefaultAsync<T>`|
