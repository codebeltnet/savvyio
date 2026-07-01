---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven.AmazonEventBusOptions`1
example:
- *content
---
`AmazonEventBusOptions<TMarker>` configures the DI-registered SNS/SQS event bus with AWS credentials and the source queue URL. The options are passed through the `AddAmazonEventBus` setup delegate.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService;
using Savvyio.Extensions.SimpleQueueService.EventDriven;

namespace ExampleApp;

public class DiEventBusRegistration
{
    public static AmazonEventBusOptions CreateAndInspect()
    {
        var options = new AmazonEventBusOptions
        {
            Credentials = new AnonymousAWSCredentials(),
            Endpoint = RegionEndpoint.EUWest1,
            SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/events")
        };
        Console.WriteLine($"Event queue: {options.SourceQueue}");
        return options;
    }
}
```



