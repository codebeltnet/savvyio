---
uid: Savvyio.Queries
summary: *content
---
Use the `Savvyio.Queries` namespace to model the read side of a CQRS application. A query represents a request for data that does not change state — implement `Query<TResult>` for typed result queries, then register a `QueryHandler` to produce the answer.

Start with `Query<TResult>` for your query payload classes. Register a handler that extends `QueryHandler<TQuery, TResult>`, and configure dispatching with `SavvyioOptions.AddQueryDispatcher` and `AddQueryHandler`. Route queries through `QueryDispatcher` or the higher-level `Mediator` from `Savvyio.Extensions`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|SavvyioOptions|⬇️|`AddQueryHandler<TImplementation>`, `AddQueryDispatcher`|
