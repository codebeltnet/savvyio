---
uid: Savvyio.Handlers.RequestReplyRegistryExtensions
example:
- *content
---
`RequestReplyRegistryExtensions.RegisterAsync<TRequest, TResponse>` simplifies async request-reply handler registration. Instead of the full `Func<T, CancellationToken, Task<TResponse>>` delegate, you pass a `Func<T, Task<TResponse>>` and the extension wraps it. The example registers a handler that returns an order status string, exercises the async path, and confirms it was selected over the synchronous overload.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class RequestReplyRegistryExtensionsExample
{
    public bool Register()
    {
        var registry = new RecordingRequestReplyRegistry();
        registry.RegisterAsync<GetOrderQuery, string>(async query =>
        {
            await Task.CompletedTask;
            return "Order " + query.OrderId;
        });
        return registry.RegisterAsyncCalled;
    }
}

public sealed record GetOrderQuery(string OrderId) : Request;

public sealed class RecordingRequestReplyRegistry : IRequestReplyRegistry<IRequest>
{
    public bool RegisterAsyncCalled { get; private set; }

    public void Register<T, TResponse>(Func<T, TResponse> handler) where T : class, IRequest { }

    public void RegisterAsync<T, TResponse>(Func<T, CancellationToken, Task<TResponse>> handler) where T : class, IRequest
    {
        RegisterAsyncCalled = true;
    }
}
```

