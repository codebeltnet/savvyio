---
uid: Savvyio.Queries.QueryDispatcher
example:
- *content
---
This example shows how to route a request-reply query through the built-in query dispatcher. The handler returns a computed order total, which gives the caller an observable outcome from the dispatcher workflow.

```csharp
using Savvyio;
using Savvyio.Dispatchers;
using Savvyio.Handlers;
using Savvyio.Queries;

namespace ExampleApp;

public sealed class QueryDispatcherExample
{
    public decimal Query()
    {
        var handler = new GetOrderTotalHandler();
        var dispatcher = new QueryDispatcher(new ServiceLocator(serviceType => serviceType == typeof(IQueryHandler) ? new object[] { handler } : []));
        return dispatcher.Query(new GetOrderTotalQuery("ORD-42"));
    }
}

public sealed class GetOrderTotalHandler : IQueryHandler
{
    public IRequestReplyActivator<IQuery> Delegates => HandlerFactory.CreateRequestReply<IQuery>(registry => registry.Register<GetOrderTotalQuery, decimal>(query => query.OrderId.Length * 10m));
}

public sealed record GetOrderTotalQuery(string OrderId) : Request, IQuery<decimal>;
```
