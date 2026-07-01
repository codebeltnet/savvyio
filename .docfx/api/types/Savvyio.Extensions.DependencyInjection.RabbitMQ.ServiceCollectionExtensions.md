---
uid: Savvyio.Extensions.DependencyInjection.RabbitMQ.ServiceCollectionExtensions
example:
- *content
---
Register RabbitMQ command queues and event buses in the DI container with `AddRabbitMqCommandQueue` and `AddRabbitMqEventBus`. Both methods configure AMQP connection settings and register the concrete queue/bus types.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.RabbitMQ;

namespace ExampleApp;

public static class RabbitMqRegistration
{
    public static IServiceCollection Configure(IServiceCollection services)
    {
        services.AddSavvyIO();
        services.AddRabbitMqCommandQueue(options =>
        {
            options.AmqpUrl = new Uri("amqp://guest:guest@localhost:5672");
            options.QueueName = "account-commands";
            options.Durable = true;
        });
        services.AddRabbitMqEventBus(options =>
        {
            options.AmqpUrl = new Uri("amqp://guest:guest@localhost:5672");
            options.ExchangeName = "account-events";
        });
        return services;
    }
}
```
