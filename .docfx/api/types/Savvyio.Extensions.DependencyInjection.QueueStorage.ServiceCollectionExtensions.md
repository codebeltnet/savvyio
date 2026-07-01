---
uid: Savvyio.Extensions.DependencyInjection.QueueStorage.ServiceCollectionExtensions
example:
- *content
---
Register Azure Queue Storage command queues and event buses in the DI container with `AddAzureCommandQueue` and `AddAzureEventBus`.

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.QueueStorage;

namespace ExampleApp;

public static class AzureQueueRegistration
{
    public static IServiceCollection Configure(IServiceCollection services)
    {
        services.AddSavvyIO();
        services.AddAzureCommandQueue(
            o => { o.ConnectionString = "UseDevelopmentStorage=true"; o.QueueName = "commands"; });
        services.AddAzureEventBus(
            azureQueueSetup: o => { o.ConnectionString = "UseDevelopmentStorage=true"; o.QueueName = "events"; },
            azureEventBusSetup: o => { o.TopicEndpoint = new Uri("https://myeventgridtopic.westeurope-1.eventgrid.azure.net/api/events"); });
        return services;
    }
}
```

