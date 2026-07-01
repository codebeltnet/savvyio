---
uid: Savvyio.Data
summary: *content
---
Use the `Savvyio.Data` namespace to define infrastructure-agnostic data access contracts. The interfaces here describe what your repositories and data stores must be able to do — read, write, delete, search, and persist — without tying your domain model to a specific database or ORM.

Start with `IPersistentDataStore<T, TOptions>` for a full-lifecycle data store, or compose narrower contracts: `IReadableDataStore<T>`, `IWritableDataStore<T>`, `IDeletableDataStore<T>`, and `ISearchableDataStore<T>`. Implementations for Entity Framework Core and Dapper are available in their respective extension packages.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
