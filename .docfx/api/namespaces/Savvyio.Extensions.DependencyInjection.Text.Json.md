---
uid: Savvyio.Extensions.DependencyInjection.Text.Json
summary: *content
---
Serializing commands and events with System.Text.Json requires a single registration call. The `Savvyio.Extensions.DependencyInjection.Text.Json` namespace provides `AddJsonMarshaller` to register `JsonMarshaller` as the `IMarshaller` used throughout the dispatch pipeline.

Start with `AddJsonMarshaller` to configure the `JsonSerializerOptions` and converters in one step. To use Newtonsoft.Json instead, replace this with `Savvyio.Extensions.DependencyInjection.Newtonsoft.Json`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddJsonMarshaller`|
