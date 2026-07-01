---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands.AmazonCommandQueueOptions`1
example:
- *content
---
`AmazonCommandQueueOptions<TMarker>` configures the DI-registered SQS command queue with AWS credentials, endpoint, and the SQS queue URL. The options are passed through the `AddAmazonCommandQueue` setup delegate.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.SimpleQueueService;
using Savvyio.Extensions.SimpleQueueService.Commands;

namespace ExampleApp;

public class DiCommandQueueRegistration
{
    public static AmazonCommandQueueOptions CreateAndInspect()
    {
        var options = new AmazonCommandQueueOptions
        {
            Credentials = new AnonymousAWSCredentials(),
            Endpoint = RegionEndpoint.EUWest1,
            SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/commands")
        };
        Console.WriteLine($"Queue: {options.SourceQueue}");
        return options;
    }
}
```



