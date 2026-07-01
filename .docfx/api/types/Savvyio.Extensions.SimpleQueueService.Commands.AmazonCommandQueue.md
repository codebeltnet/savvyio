---
uid: Savvyio.Extensions.SimpleQueueService.Commands.AmazonCommandQueue
example:
- *content
---
`AmazonCommandQueue` sends and receives commands through Amazon SQS. Configure it with `AmazonCommandQueueOptions` and use it as `IPointToPointChannel<ICommand>`.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Savvyio.Commands;
using Savvyio.Extensions.SimpleQueueService.Commands;
using Savvyio.Messaging;

namespace ExampleApp;

public class SqsCommandQueueConfig
{
    public static AmazonCommandQueueOptions CreateOptions() => new AmazonCommandQueueOptions { Credentials = new AnonymousAWSCredentials(), Endpoint = RegionEndpoint.EUWest1, SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/commands") };
    public static IPointToPointChannel<ICommand> AsChannel(AmazonCommandQueue q) => q;
}
```
