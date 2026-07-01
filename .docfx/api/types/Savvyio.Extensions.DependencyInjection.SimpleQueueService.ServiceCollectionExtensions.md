---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.ServiceCollectionExtensions
example:
- *content
---
Register Amazon SQS command queues and SNS event buses in the DI container with `AddAmazonCommandQueue` and `AddAmazonEventBus`. Both methods configure AWS credentials and resource settings.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService;

namespace ExampleApp;

public static class AmazonRegistration
{
    public static IServiceCollection Configure(IServiceCollection services)
    {
        services.AddSavvyIO();
        services.AddAmazonCommandQueue(options =>
        {
            options.Credentials = new AnonymousAWSCredentials();
            options.Endpoint = RegionEndpoint.EUWest1;
            options.SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/account-commands");
        });
        services.AddAmazonEventBus(options =>
        {
            options.Credentials = new AnonymousAWSCredentials();
            options.Endpoint = RegionEndpoint.EUWest1;
            options.SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/account-events");
        });
        return services;
    }
}
```
