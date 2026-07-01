---
uid: Savvyio.Extensions.DependencyInjection.SimpleQueueService
summary: *content
---
Amazon SQS/SNS messaging for Savvy I/O is registered in one call per channel. The `Savvyio.Extensions.DependencyInjection.SimpleQueueService` namespace provides `AddAmazonCommandQueue<TMarker>` and `AddAmazonEventBus<TMarker>` to configure the AWS credentials, region, and resource names and register the corresponding service.

Start with `AddAmazonCommandQueue<TMarker>` to register an SQS command queue, and `AddAmazonEventBus<TMarker>` for an SNS/SQS event bus. Choose this namespace when your application runs on AWS and requires SQS-based command queuing or SNS/SQS fan-out for integration events; for Azure, use `Savvyio.Extensions.DependencyInjection.QueueStorage`, and for self-hosted brokers, consider `Savvyio.Extensions.DependencyInjection.RabbitMQ` or `Savvyio.Extensions.DependencyInjection.NATS`. The concrete types are in `Savvyio.Extensions.DependencyInjection.SimpleQueueService.Commands` and `Savvyio.Extensions.DependencyInjection.SimpleQueueService.EventDriven`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddAmazonCommandQueue`, `AddAmazonCommandQueue<TMarker>`, `AddAmazonEventBus`, `AddAmazonEventBus<TMarker>`|
