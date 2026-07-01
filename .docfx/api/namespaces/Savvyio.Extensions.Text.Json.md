---
uid: Savvyio.Extensions.Text.Json
summary: *content
---
System.Text.Json is the modern, built-in serialization choice for Savvy I/O. `JsonMarshaller` is the `IMarshaller` implementation, and `JsonConverterExtensions` registers the converters that make `IMessage<T>`, `IMetadataDictionary`, requests, and value objects round-trip correctly through `JsonSerializerOptions`. `JsonSerializerOptionsExtensions.Clone` deep-copies options for scoped contexts.

Start with `JsonConverterExtensions.AddMessageConverter()` and `AddMetadataDictionaryConverter()` for messaging types. Add `AddRequestConverter`, `AddSingleValueObjectConverter`, `AddDateTimeConverter`, and `AddDateTimeOffsetConverter` as the domain model requires. Use `RemoveAllOf<T>` to strip conflicting converters before composing new ones. Register the marshaller through `Savvyio.Extensions.DependencyInjection.Text.Json`; for applications that already depend on Newtonsoft.Json, `Savvyio.Extensions.Newtonsoft.Json` provides equivalent coverage.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection<JsonConverter>|⬇️|`RemoveAllOf<T>`, `RemoveAllOf`, `AddMetadataDictionaryConverter`, `AddMessageConverter`, `AddRequestConverter`, `AddDateTimeConverter`, `AddDateTimeOffsetConverter`, `AddSingleValueObjectConverter`|
|JsonSerializerOptions|⬇️|`Clone`|
