---
uid: Savvyio.Extensions.SimpleQueueService.EventDriven.AmazonEventBusOptions
example:
- *content
---
Configure `AmazonEventBusOptions` for an SNS/SQS event bus by providing AWS credentials, endpoint, and source queue URL. Adjust the polling timeout and visibility timeout via `ReceiveContext`.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Savvyio.Extensions.SimpleQueueService.EventDriven;

namespace ExampleApp;

public class AmazonEventBusConfig
{
    public static AmazonEventBusOptions CreateOptions()
    {
        var options = new AmazonEventBusOptions
        {
            Credentials = new AnonymousAWSCredentials(),
            Endpoint = RegionEndpoint.EUWest1,
            SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/account-events")
        };
        options.ReceiveContext.PollingTimeout = TimeSpan.FromSeconds(20);
        options.ReceiveContext.VisibilityTimeout = TimeSpan.FromSeconds(30);
        return options;
    }
}
```


