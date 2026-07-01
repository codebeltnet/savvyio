---
uid: Savvyio.Extensions.DependencyInjection.Newtonsoft.Json
summary: *content
---
Serializing commands and events with Newtonsoft.Json requires a single registration call. The `Savvyio.Extensions.DependencyInjection.Newtonsoft.Json` namespace provides `AddNewtonsoftJsonMarshaller` to register `NewtonsoftJsonMarshaller` as the `IMarshaller` used throughout the dispatch pipeline.

Start with `AddNewtonsoftJsonMarshaller` to configure the serializer settings and converters in one step. To use System.Text.Json instead, replace this with `Savvyio.Extensions.DependencyInjection.Text.Json`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddNewtonsoftJsonMarshaller`|
