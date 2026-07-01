---
uid: Savvyio.Extensions.SimpleQueueService.EventDriven.StringExtensions
example:
- *content
---
Use `StringExtensions.ToSnsUri` to convert an SNS topic name to a `Uri` formatted as an Amazon Resource Name (ARN) for use as the event bus source.

```csharp
using System;
using Savvyio.Extensions.SimpleQueueService.EventDriven;

namespace ExampleApp;

public class SnsUriExample
{
    public static Uri BuildSnsTopicUri()
    {
        var topicArn = "account-events".ToSnsUri(options =>
        {
            options.Partition = "aws";
            options.Region = "eu-west-1";
            options.AccountId = "123456789012";
        });
        return topicArn;
    }
}
```
