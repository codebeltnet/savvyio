---
uid: Savvyio.Extensions.Text.Json.Converters
summary: *content
---
Each converter type in this namespace handles a specific Savvy I/O type: `MessageConverter` for `IMessage<T>`, `MetadataDictionaryConverter` for metadata dictionaries, `DateTimeConverter` and `DateTimeOffsetConverter` for temporal values, `RequestConverter` for `IRequest`, and `SingleValueObjectConverter` for primitive value objects. Each is a concrete `JsonConverter<T>` that you can add directly to `JsonSerializerOptions.Converters`.

Start with `MessageConverter` and `MetadataDictionaryConverter` for messaging types. Choose the individual converter classes from this namespace when you need explicit control over which converters are active or when you need to compose them with other `JsonConverter<T>` implementations; for one-call registration of all needed converters, use the extension methods in `Savvyio.Extensions.Text.Json` instead.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
