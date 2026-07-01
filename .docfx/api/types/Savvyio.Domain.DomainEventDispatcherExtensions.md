---
uid: Savvyio.Domain.DomainEventDispatcherExtensions
example:
- *content
---
This example shows how to flush the pending events collected by an aggregate through IDomainEventDispatcher. The sample raises one batch synchronously and a second batch asynchronously so both RaiseMany and RaiseManyAsync are exercised in the workflow that clears the aggregate event list.

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Dispatchers;
using Savvyio.Domain;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class DomainEventDispatcherExtensionsExample
{
    public async Task<(bool SyncCleared, bool AsyncCleared)> RaisePendingEventsAsync()
    {
        var dispatcher = new DomainEventDispatcher(new ServiceLocator(serviceType => serviceType == typeof(IDomainEventHandler) ? new object[] { new AccountOpenedHandler() } : []));

        var syncAggregate = new AccountAggregate();
        syncAggregate.Record(new AccountOpenedEvent("ACC-42"));
        dispatcher.RaiseMany(syncAggregate);

        var asyncAggregate = new AccountAggregate();
        asyncAggregate.Record(new AccountOpenedEvent("ACC-43"));
        await dispatcher.RaiseManyAsync(asyncAggregate).ConfigureAwait(false);

        return (syncAggregate.Events.Count == 0, asyncAggregate.Events.Count == 0);
    }
}

public sealed class AccountAggregate : IAggregateRoot<AccountOpenedEvent>
{
    private readonly List<AccountOpenedEvent> _events = new();
    public IMetadataDictionary Metadata { get; } = new MetadataDictionary();
    public IReadOnlyList<AccountOpenedEvent> Events => _events;
    public void Record(AccountOpenedEvent e) => _events.Add(e);
    public void RemoveAllEvents() => _events.Clear();
}

public sealed class AccountOpenedHandler : IDomainEventHandler
{
    public IFireForgetActivator<IDomainEvent> Delegates => HandlerFactory.CreateFireForget<IDomainEvent>(registry => registry.Register<AccountOpenedEvent>(_ => { }));
}

public sealed record AccountOpenedEvent(string AccountId) : Request, IDomainEvent;
```
