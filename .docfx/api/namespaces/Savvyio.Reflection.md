---
uid: Savvyio.Reflection
summary: *content
---
Use the `Savvyio.Reflection` namespace when you need low-level assembly inspection within the Savvy I/O framework. It provides `AssemblyContext`, which encapsulates metadata about an assembly that the handler discovery and registration infrastructure inspects at startup.

`AssemblyContext` is used internally by the handler services descriptor to map handler implementations to their assembly origins. You rarely need to create it directly; it is populated automatically when you call `AddHandlerServicesDescriptor` during DI setup.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]
