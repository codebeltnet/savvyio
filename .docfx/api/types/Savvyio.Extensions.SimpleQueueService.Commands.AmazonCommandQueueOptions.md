---
uid: Savvyio.Extensions.SimpleQueueService.Commands.AmazonCommandQueueOptions
example:
- *content
---
Configure `AmazonCommandQueueOptions` for a command queue by setting AWS credentials, the source queue URL, and fine-tuning the receive behavior through `ReceiveContext`.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Savvyio.Extensions.SimpleQueueService.Commands;

namespace ExampleApp;

public class AmazonCommandQueueConfig
{
    public static AmazonCommandQueueOptions CreateOptions()
    {
        var options = new AmazonCommandQueueOptions
        {
            Credentials = new AnonymousAWSCredentials(),
            Endpoint = RegionEndpoint.EUWest1,
            SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/account-commands")
        };
        options.ReceiveContext.NumberOfMessagesToTakePerRequest = 5;
        options.ReceiveContext.RemoveProcessedMessages = true;
        return options;
    }
}
```

