---
uid: Savvyio.Handlers.FireForgetRegistryExtensions
example:
- *content
---
`FireForgetRegistryExtensions.RegisterAsync<TRequest>` simplifies async fire-and-forget handler registration. Instead of passing the full `Func<T, CancellationToken, Task>` delegate with a cancellation token, you pass a simpler `Func<T, Task>` and the extension wraps it for the registry. The example creates a registry stub that records whether the async path was taken, registers a handler for `CreateOrderCommand`, and verifies the registration result.

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using Savvyio;
using Savvyio.Handlers;

namespace ExampleApp;

public sealed class FireForgetRegistryExtensionsExample
{
    public bool Register()
    {
        var registry = new RecordingFireForgetRegistry();
        registry.RegisterAsync<CreateOrderCommand>(async cmd =>
        {
            Console.WriteLine("Processing: " + cmd.OrderId);
            await Task.CompletedTask;
        });
        return registry.RegisterAsyncCalled;
    }
}

public sealed record CreateOrderCommand(string OrderId) : Request;

public sealed class RecordingFireForgetRegistry : IFireForgetRegistry<IRequest>
{
    public bool RegisterAsyncCalled { get; private set; }

    public void Register<T>(Action<T> handler) where T : class, IRequest { }

    public void RegisterAsync<T>(Func<T, CancellationToken, Task> handler) where T : class, IRequest
    {
        RegisterAsyncCalled = true;
    }
}
```

