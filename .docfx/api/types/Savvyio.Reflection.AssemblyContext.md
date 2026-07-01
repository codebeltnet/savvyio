---
uid: Savvyio.Reflection.AssemblyContext
example:
- *content
---
This example shows how to narrow assembly discovery to application assemblies before scanning for Savvy I/O handlers.

```csharp
using System;
using System.Linq;
using Savvyio.Reflection;

namespace ExampleApp;

public sealed class AssemblyContextExample
{
    public string[] GetApplicationAssemblies()
    {
        AssemblyContext.AssemblyFilterCallback = assembly => assembly.GetName().Name?.StartsWith("ExampleApp", StringComparison.Ordinal) == true;
        AssemblyContext.AssemblyDependenciesFilterCallback = assemblyName => assemblyName.Name?.StartsWith("ExampleApp", StringComparison.Ordinal) == true;
        AssemblyContext.AssemblyDependenciesCallback = assembly => new[] { assembly };
        return AssemblyContext.CurrentDomainAssemblies.Select(a => a.GetName().Name ?? string.Empty).ToArray();
    }
}
```
