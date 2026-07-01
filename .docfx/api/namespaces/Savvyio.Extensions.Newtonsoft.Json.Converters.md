---
uid: Savvyio.Extensions.Newtonsoft.Json.Converters
summary: *content
---
`AggregateRootConverter`, `MessageConverter`, `RequestConverter`, `SingleValueObjectConverter`, and `ValueObjectConverter` — these five concrete `JsonConverter` types handle Savvy I/O domain-model serialization for Newtonsoft.Json. Each targets one base-type contract and can be composed with third-party converters or subclassed when custom serialization behavior is needed.

Start with `MessageConverter` because `IMessage<T>` is the outermost envelope for command and event payloads. Add the remaining converters as the domain model requires. For most applications, the extension methods in `Savvyio.Extensions.Newtonsoft.Json` register the needed subset in fewer lines; reference individual converter types here only when you need to subclass or override one.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
