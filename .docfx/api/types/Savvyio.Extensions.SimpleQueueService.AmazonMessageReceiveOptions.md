---
uid: Savvyio.Extensions.SimpleQueueService.AmazonMessageReceiveOptions
example:
- *content
---
`AmazonMessageReceiveOptions` controls SQS polling behavior — number of messages, visibility timeout, polling timeout, and whether to remove processed messages. Access it through `AmazonMessageOptions.ReceiveContext`.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Savvyio.Extensions.SimpleQueueService;

namespace ExampleApp;

public class AmazonReceiveSetup
{
    public static AmazonMessageOptions CreateOptions()
    {
        var options = new AmazonMessageOptions
        {
            Credentials = new AnonymousAWSCredentials(),
            Endpoint = RegionEndpoint.EUWest1,
            SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/account-commands")
        };
        AmazonMessageReceiveOptions receiveOptions = options.ReceiveContext;
        receiveOptions.NumberOfMessagesToTakePerRequest = 5;
        receiveOptions.VisibilityTimeout = TimeSpan.FromSeconds(30);
        receiveOptions.PollingTimeout = TimeSpan.FromSeconds(20);
        receiveOptions.RemoveProcessedMessages = true;
        return options;
    }
}
```

