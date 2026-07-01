---
uid: Savvyio.Commands
summary: *content
---
Use the `Savvyio.Commands` namespace to model and dispatch write-side operations in a CQRS application. A command represents intent to change state — implement `Command` as your base class when you want a concrete, serializable command payload, then register a `CommandHandler` to process it.

Start with `Command` for your command payload classes. Register a handler that extends `CommandHandler<TCommand>`, and use `SavvyioOptions.AddCommandDispatcher` and `SavvyioOptions.AddCommandHandler` to wire them into the DI container. Route commands through `CommandDispatcher` or use the higher-level `Mediator` from `Savvyio.Extensions`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|SavvyioOptions|⬇️|`AddCommandHandler<TImplementation>`, `AddCommandDispatcher`|
