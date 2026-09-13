#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;
using Xunit.v3;
using Xunit.v3.Priority;

namespace Savvyio;

// xUnit 4 orders methods separately from cases, but Xunit.v3.Priority only ships a case orderer.
public sealed class PriorityMethodOrderer : ITestMethodOrderer
{
    private static readonly ConcurrentDictionary<string, int> DefaultPriorities = new(StringComparer.Ordinal);

    public IReadOnlyCollection<TTestMethod?> OrderTestMethods<TTestMethod>(IReadOnlyCollection<TTestMethod?> testMethods)
        where TTestMethod : notnull, ITestMethod
    {
        return testMethods
            .OrderBy(method => method is IXunitTestMethod xunitMethod ? GetPriority(xunitMethod) : int.MaxValue)
            .ThenBy(method => method?.MethodName, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static int GetPriority(IXunitTestMethod method)
    {
        return method.Method.GetCustomAttribute<PriorityAttribute>()?.Priority ?? GetDefaultPriority(method.TestClass);
    }

    private static int GetDefaultPriority(IXunitTestClass testClass)
    {
        return DefaultPriorities.GetOrAdd(testClass.Class.Name,
            _ => testClass.Class.GetCustomAttribute<DefaultPriorityAttribute>()?.Priority ?? int.MaxValue);
    }
}
