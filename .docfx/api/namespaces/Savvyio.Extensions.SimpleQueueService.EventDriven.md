---
uid: Savvyio.Extensions.SimpleQueueService.EventDriven
summary: *content
---
Savvy I/O integration events published via Amazon SNS/SQS are handled by this namespace. `AmazonEventBus` implements `IEventBus<T>` by publishing to an SNS topic and polling the subscribed SQS queue for received events. `AmazonEventBusOptions` configures the SNS topic ARN, SQS queue URL, AWS credentials, and serialization.

Start with `AmazonEventBus` as the implementation class for SNS/SQS event delivery. Choose this namespace when you need the concrete event-bus type to extend, test, or configure directly; for DI-based registration, use `Savvyio.Extensions.DependencyInjection.SimpleQueueService.AddAmazonEventBus<TMarker>`. `StringExtensions.ToSnsUri` converts a topic ARN to an SNS-compatible `Uri`. For the corresponding SQS command queue, see `Savvyio.Extensions.SimpleQueueService.Commands`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|string|⬇️|`ToSnsUri`|
