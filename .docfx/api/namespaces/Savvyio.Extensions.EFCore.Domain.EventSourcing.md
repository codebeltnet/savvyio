---
uid: Savvyio.Extensions.EFCore.Domain.EventSourcing
summary: *content
---
EF Core–backed event sourcing requires both a schema setup and a repository that writes individual event rows. The `Savvyio.Extensions.EFCore.Domain.EventSourcing` namespace provides all three: the EF Core entity, the repository, and the `ModelBuilder` extension that creates the event-store table.

Start with `ModelBuilderExtensions.AddEventSourcing<TEntity, TKey>` in `OnModelCreating` to create the event table. Then use `EfCoreTracedAggregateRepository<TAggregateRoot, TKey, TContext>` as the aggregate repository. Use `EfCoreTracedAggregateEntityExtensions.ToTracedDomainEvent<TEntity, TKey>` to hydrate domain events from stored rows, and `TracedDomainEventExtensions.ToByteArray` to serialize a traced event for storage. Register with `Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|EfCoreTracedAggregateEntity<TEntity, TKey>|⬇️|`ToTracedDomainEvent<TEntity, TKey>`|
|ITracedDomainEvent|⬇️|`ToByteArray`|
|ModelBuilder|⬇️|`AddEventSourcing<TEntity, TKey>`|
