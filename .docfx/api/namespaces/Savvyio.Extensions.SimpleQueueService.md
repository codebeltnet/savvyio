---
uid: Savvyio.Extensions.SimpleQueueService
summary: *content
---
Amazon SQS/SNS support in Savvy I/O is organized around the `AmazonQueue<T>` and `AmazonBus<T>` base classes. The `Savvyio.Extensions.SimpleQueueService` namespace provides those base classes, the `AmazonMessage<T>` envelope, and options types for configuring AWS credentials, region, and resource names.

Start with `AmazonMessageOptions` to configure the SQS queue URL or SNS topic ARN along with AWS credentials and region. The concrete command queue is in `Savvyio.Extensions.SimpleQueueService.Commands` and the event bus in `Savvyio.Extensions.SimpleQueueService.EventDriven`. Register both with `Savvyio.Extensions.DependencyInjection.SimpleQueueService`. `ClientConfigExtensions` provides helpers for validating the AWS client configuration before use.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IClientConfig|⬇️|`IsValid`, `SimpleQueueService`, `SimpleNotificationService`|
