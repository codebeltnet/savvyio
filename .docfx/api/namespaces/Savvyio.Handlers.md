---
uid: Savvyio.Handlers
summary: *content
---
Use the `Savvyio.Handlers` namespace to build the handler layer of a CQRS application. It provides the registry contracts `IFireForgetRegistry` (for command and domain event handlers) and `IRequestReplyRegistry` (for query handlers), together with activator and handler base interfaces. The `OrphanedHandlerException` signals that a request reached a dispatcher with no registered handler.

Start with `IFireForgetRegistry<TRequest>` for fire-and-forget command or event handlers and `IRequestReplyRegistry<TRequest>` for request-reply query handlers. Use `RegisterAsync` on either registry to subscribe handler delegates. If you use automatic handler discovery, configure it through `SavvyioOptions` in `Savvyio.Extensions.DependencyInjection`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IFireForgetRegistry<TRequest>|⬇️|`RegisterAsync<TRequest>`|
|IRequestReplyRegistry<TRequest>|⬇️|`RegisterAsync<TRequest, TResponse>`|
