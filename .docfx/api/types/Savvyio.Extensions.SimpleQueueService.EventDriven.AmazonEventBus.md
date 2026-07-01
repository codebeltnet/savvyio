---
uid: Savvyio.Extensions.SimpleQueueService.EventDriven.AmazonEventBus
example:
- *content
---
`AmazonEventBus` publishes and subscribes to integration events through Amazon SNS/SQS. Configure it with `AmazonEventBusOptions` and use it as `IPublishSubscribeChannel<IIntegrationEvent>`.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Savvyio.EventDriven;
using Savvyio.Extensions.SimpleQueueService.EventDriven;
using Savvyio.Messaging;

namespace ExampleApp;

public class SnsEventBusConfig
{
    public static AmazonEventBusOptions CreateOptions() => new AmazonEventBusOptions { Credentials = new AnonymousAWSCredentials(), Endpoint = RegionEndpoint.EUWest1, SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/events") };
    public static IPublishSubscribeChannel<IIntegrationEvent> AsChannel(AmazonEventBus b) => b;
}
```
