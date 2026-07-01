---
uid: Savvyio.Extensions.SimpleQueueService.AmazonMessageOptions
example:
- *content
---
Configure an `AmazonMessageOptions` with the AWS credentials, endpoint region, and SQS queue URL required for Amazon SQS message delivery.

```csharp
using System;
using Amazon;
using Amazon.Runtime;
using Savvyio.Extensions.SimpleQueueService;

namespace ExampleApp;

public class AmazonSetup
{
    public static AmazonMessageOptions CreateOptions()
    {
        var options = new AmazonMessageOptions
        {
            Credentials = new AnonymousAWSCredentials(),
            Endpoint = RegionEndpoint.EUWest1,
            SourceQueue = new Uri("https://sqs.eu-west-1.amazonaws.com/123456789012/account-commands")
        };
        return options;
    }
}
```
