---
uid: Savvyio.Extensions.DependencyInjection.Messaging.ServiceCollectionExtensions
example:
- *content
---
Registering transport-agnostic messaging in Savvy I/O uses `AddMessageQueue` for point-to-point command delivery and `AddMessageBus` for publish-subscribe event delivery. Both methods accept the service interface and the concrete implementation type, enabling seamless transport swapping between in-memory, RabbitMQ, NATS, and cloud brokers. The example registers both channels with concrete implementations and resolves each through its abstract interface.

```csharp
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Commands;
using Savvyio.EventDriven;
using Savvyio.Extensions.DependencyInjection.Messaging;
using Savvyio.Messaging;

namespace ExampleApp;

public static class MessagingRegistration
{
    public static IServiceCollection AddMessagingChannels(this IServiceCollection services)
    {
        services.AddMessageQueue<OrderCommandQueue, ICommand>();
        services.AddMessageBus<OrderEventBus, IIntegrationEvent>();
        return services;
    }
}

public sealed class OrderCommandQueue : IPointToPointChannel<ICommand>
{
    public async IAsyncEnumerable<IMessage<ICommand>> ReceiveAsync(Action<AsyncOptions> setup = null)
    {
        await Task.CompletedTask;
        yield break;
    }

    public Task SendAsync(IEnumerable<IMessage<ICommand>> messages, Action<AsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }
}

public sealed class OrderEventBus : IPublishSubscribeChannel<IIntegrationEvent>
{
    public Task PublishAsync(IMessage<IIntegrationEvent> message, Action<AsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }

    public Task SubscribeAsync(Func<IMessage<IIntegrationEvent>, CancellationToken, Task> asyncHandler, Action<SubscribeAsyncOptions> setup = null)
    {
        return Task.CompletedTask;
    }
}
```
