---
uid: Savvyio.Queries.SavvyioOptionsExtensions
example:
- *content
---
This example shows how to configure SavvyioOptions for a query-driven application service. The options register both the query handler implementation and the default query dispatcher so request-reply operations can resolve through the same configuration object.

```csharp
using Savvyio;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace ExampleApp;

public sealed class QueryOptionsExtensionsExample
{
    public int Configure()
    {
        var options = new SavvyioOptions().AddQueryHandler<GetOrderTotalHandler>().AddQueryDispatcher();
        return options.HandlerImplementationTypes.Count + options.DispatcherImplementationTypes.Count;
    }
}

public sealed class GetOrderTotalHandler : IQueryHandler
{
    public IRequestReplyActivator<IQuery> Delegates => HandlerFactory.CreateRequestReply<IQuery>(registry => registry.Register<GetOrderTotalQuery, decimal>(_ => 42m));
}

public sealed record GetOrderTotalQuery(string OrderId) : Request, IQuery<decimal>;
```
