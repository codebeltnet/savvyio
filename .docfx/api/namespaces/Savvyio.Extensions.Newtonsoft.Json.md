---
uid: Savvyio.Extensions.Newtonsoft.Json
summary: *content
---
Round-tripping Savvy I/O domain types — `IMessage<T>`, aggregate roots, value objects, and requests — through Newtonsoft.Json requires converters that understand each type's custom serialization contract. This namespace contains `NewtonsoftJsonMarshaller`, `JsonConverterExtensions`, and `JsonSerializerExtensions` for that purpose.

`JsonConverterExtensions` is the registration API: `AddMessageConverter()` and `AddMetadataDictionaryConverter()` cover messaging types, while `AddAggregateRootConverter<TKey>`, `AddValueObjectConverter`, `AddSingleValueObjectConverter`, and `AddRequestConverter` cover domain-model types. `JsonSerializerExtensions.ResolvePropertyKeyByConvention` and `ResolveDictionaryKeyByConvention` align key naming with the Savvy I/O conventions. Prefer this namespace when the application already depends on Newtonsoft.Json or requires features not available in System.Text.Json; for greenfield apps, `Savvyio.Extensions.Text.Json` requires no additional package. Register the marshaller via `Savvyio.Extensions.DependencyInjection.Newtonsoft.Json`.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection<JsonConverter>|⬇️|`AddValueObjectConverter`, `AddAggregateRootConverter<TKey>`, `AddMetadataDictionaryConverter`, `AddRequestConverter`, `AddMessageConverter`, `AddSingleValueObjectConverter`|
|JsonSerializer|⬇️|`ResolvePropertyKeyByConvention`, `ResolveDictionaryKeyByConvention`|
