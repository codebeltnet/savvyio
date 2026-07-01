---
uid: Savvyio.Extensions.SimpleQueueService.AmazonResourceNameOptions
example:
- *content
---
`AmazonResourceNameOptions` builds Amazon Resource Names (ARNs) for SQS queues and SNS topics using the configured partition, region, and account ID.

```csharp
using Savvyio.Extensions.SimpleQueueService;

namespace ExampleApp;

public class AmazonResourceNameSetup
{
    public static string BuildQueueArn(string queueName)
    {
        var options = new AmazonResourceNameOptions
        {
            Partition = "aws",
            Region = "eu-west-1",
            AccountId = "123456789012"
        };
        return $"arn:{options.Partition}:sqs:{options.Region}:{options.AccountId}:{queueName}";
    }
}
```
