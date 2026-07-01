---
uid: Savvyio.Diagnostics
summary: *content
---
Use the `Savvyio.Diagnostics` namespace to add health monitoring to a Savvy I/O application. `IHealthCheckProvider` and `IAsyncHealthCheckProvider` define synchronous and asynchronous health-check contracts that infrastructure components can implement to report their operational status.

Start with `IAsyncHealthCheckProvider` when the health check involves I/O — for example, verifying that a database connection is available or that a message broker is reachable. Use `IHealthCheckProvider` for lightweight, synchronous checks. Both interfaces integrate with standard health check pipelines.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
