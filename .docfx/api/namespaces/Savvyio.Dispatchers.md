---
uid: Savvyio.Dispatchers
summary: *content
---
Use the `Savvyio.Dispatchers` namespace to route commands, queries, and events to their registered handlers. The dispatcher layer decouples the caller from the handler — the caller sends a request to the dispatcher and the framework locates the correct handler.

Start with `FireForgetDispatcher` for commands and domain events (no return value expected) and `RequestReplyDispatcher` for queries that return a result. Both extend the base `Dispatcher` and use a `ServiceLocator` to resolve handlers from the DI container. For a unified entry point that routes all request types, use `Mediator` from the `Savvyio.Extensions` namespace.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
