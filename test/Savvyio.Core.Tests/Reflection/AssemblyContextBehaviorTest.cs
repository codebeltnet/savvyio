#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Codebelt.Extensions.Xunit;
using Cuemon;
using Savvyio.Commands;
using Xunit;

namespace Savvyio.Reflection;

public class AssemblyContextBehaviorTest : Test
{
    public AssemblyContextBehaviorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("System.Application", false)]
    [InlineData("Microsoft.Application", false)]
    [InlineData("Application.System", true)]
    [InlineData("system.Application", true)]
    [InlineData("microsoft.Application", true)]
    public void AssemblyFilterCallback_ShouldUseOrdinalPrefixFiltering(string? fullName, bool expected)
    {
        using var context = new IsolatedAssemblyContext();

        Assert.Equal(expected, context.AssemblyFilter(new ReferencedAssembly(fullName)));
    }

    [Theory]
    [InlineData("System.Application", false)]
    [InlineData("Microsoft.Application", false)]
    [InlineData("Application.System", true)]
    [InlineData("system.Application", true)]
    [InlineData("microsoft.Application", true)]
    public void AssemblyDependenciesFilterCallback_ShouldUseOrdinalPrefixFiltering(string name, bool expected)
    {
        using var context = new IsolatedAssemblyContext();

        Assert.Equal(expected, context.ReferenceFilter(new AssemblyName(name)));
    }

    [Theory]
    [InlineData(nameof(AssemblyContext.AssemblyFilterCallback))]
    [InlineData(nameof(AssemblyContext.AssemblyDependenciesCallback))]
    [InlineData(nameof(AssemblyContext.AssemblyDependenciesFilterCallback))]
    public void Callback_ShouldRejectNullAndPreservePreviousDelegate(string propertyName)
    {
        using var context = new IsolatedAssemblyContext();
        var property = context.Type.GetProperty(propertyName)!;
        var original = property.GetValue(null);

        var error = Assert.Throws<TargetInvocationException>(() => property.SetValue(null, null));

        Assert.Equal("value", Assert.IsType<ArgumentNullException>(error.InnerException).ParamName);
        Assert.Same(original, property.GetValue(null));
    }

    [Fact]
    public void AssemblyDependenciesCallback_ShouldTraverseTransitiveReferences()
    {
        using var context = new IsolatedAssemblyContext();
        var root = typeof(Command).Assembly;
        var core = typeof(AssemblyContext).Assembly;
        var transitive = typeof(Cuemon.Reflection.AssemblyContext).Assembly;
        // Commands references Savvyio.Core, which in turn references Cuemon.Core.
        Assert.DoesNotContain(root.GetReferencedAssemblies(), name => name.FullName == transitive.FullName);

        var assemblies = context.Dependencies(root).ToList();

        Assert.Equal(root, assemblies[0]);
        Assert.Contains(assemblies, assembly => assembly.FullName == core.FullName);
        Assert.Contains(assemblies, assembly => assembly.FullName == transitive.FullName);
        Assert.Equal(assemblies.Count, assemblies.Distinct().Count());
        Assert.DoesNotContain(assemblies, assembly => assembly.FullName!.StartsWith("System", StringComparison.Ordinal));
        Assert.DoesNotContain(assemblies, assembly => assembly.FullName!.StartsWith("Microsoft", StringComparison.Ordinal));
    }

    [Fact]
    public void AssemblyDependenciesCallback_ShouldSkipSelfReferencesAndDuplicates()
    {
        using var context = new IsolatedAssemblyContext();
        var dependency = typeof(Validator).Assembly;
        var root = new ReferencedAssembly("Application.Root", new AssemblyName("Application.Root"), dependency.GetName(), dependency.GetName());
        context.ReferenceFilter = name => name.Name is "Application.Root" or "Cuemon.Kernel";

        Assert.Equal(new Assembly[] { root, dependency }, context.Dependencies(root));
    }

    [Fact]
    public void AssemblyDependenciesCallback_ShouldContinueAfterUnresolvableReference()
    {
        using var context = new IsolatedAssemblyContext();
        var dependency = typeof(Validator).Assembly;
        var root = new ReferencedAssembly("Application.Root", new AssemblyName("Savvyio.Tests.NonexistentDependency.8d9b24a2"), dependency.GetName());
        context.ReferenceFilter = name => name.Name == "Savvyio.Tests.NonexistentDependency.8d9b24a2" || name.FullName == dependency.FullName;

        Assert.Equal(new Assembly[] { root, dependency }, context.Dependencies(root));
    }

    [Fact]
    public void AssemblyDependenciesCallback_ShouldApplyCustomFilterWithoutFilteringRoot()
    {
        using var context = new IsolatedAssemblyContext();
        var root = typeof(Command).Assembly;
        var inspected = new List<AssemblyName>();
        context.ReferenceFilter = name =>
        {
            inspected.Add(name);
            return false;
        };

        Assert.Same(root, Assert.Single(context.Dependencies(root)));
        Assert.Equal(root.GetReferencedAssemblies().Select(name => name.FullName), inspected.Select(name => name.FullName));
    }

    [Fact]
    public void CurrentDomainAssemblies_ShouldFilterExpandDeduplicateAndExcludeOwnAssembly()
    {
        using var context = new IsolatedAssemblyContext();
        var root = typeof(AssemblyContextBehaviorTest).Assembly;
        var dependency = typeof(Command).Assembly;
        var expanded = new List<Assembly>();
        context.AssemblyFilter = assembly => assembly == root;
        context.Dependencies = assembly =>
        {
            expanded.Add(assembly);
            return [assembly, dependency, dependency, context.Type.Assembly];
        };

        var assemblies = context.CurrentDomainAssemblies;

        Assert.Same(root, Assert.Single(expanded));
        Assert.Equal(new[] { root, dependency }, assemblies);
        Assert.Throws<NotSupportedException>(() => ((IList<Assembly>)assemblies).Add(root));
    }

    [Fact]
    public void CurrentDomainAssemblies_ShouldCacheFirstResultDespiteCallbackChanges()
    {
        using var context = new IsolatedAssemblyContext();
        var root = typeof(AssemblyContextBehaviorTest).Assembly;
        context.AssemblyFilter = assembly => assembly == root;
        context.Dependencies = assembly => [assembly];
        var first = context.CurrentDomainAssemblies;

        context.AssemblyFilter = _ => throw new InvalidOperationException("Must not rescan.");
        context.Dependencies = _ => throw new InvalidOperationException("Must not traverse again.");
        context.ReferenceFilter = _ => throw new InvalidOperationException("Must not refilter.");

        Assert.Same(first, context.CurrentDomainAssemblies);
        Assert.Same(root, Assert.Single(first));
    }

    [Fact]
    public void CurrentDomainAssemblies_ShouldAllowEmptyResult()
    {
        using var context = new IsolatedAssemblyContext();
        context.AssemblyFilter = _ => false;
        context.Dependencies = _ => throw new InvalidOperationException("No assembly passed the filter.");

        Assert.Empty(context.CurrentDomainAssemblies);
    }

    [Fact]
    public void CurrentDomainAssemblies_ShouldCacheCallbackFailure()
    {
        using var context = new IsolatedAssemblyContext();
        var expected = new InvalidOperationException("Assembly filter failed.");
        context.AssemblyFilter = _ => throw expected;

        var first = Assert.Throws<TargetInvocationException>(() => context.CurrentDomainAssemblies);
        context.AssemblyFilter = _ => false;
        var second = Assert.Throws<TargetInvocationException>(() => context.CurrentDomainAssemblies);

        Assert.Same(expected, first.InnerException);
        Assert.Same(expected, second.InnerException);
    }

    private sealed class ReferencedAssembly(string? fullName, params AssemblyName[] references) : Assembly
    {
        public override string? FullName => fullName;

        public override AssemblyName[] GetReferencedAssemblies() => references;
    }

    // A separate load context gives each test fresh static callbacks and Lazy state.
    // This avoids resetting private fields or changing the application's global scan.
    private sealed class IsolatedAssemblyContext : IDisposable
    {
        private readonly AssemblyLoadContext _loadContext = new(null, isCollectible: true);

        public IsolatedAssemblyContext()
        {
            Type = _loadContext.LoadFromAssemblyPath(typeof(AssemblyContext).Assembly.Location)
                .GetType(typeof(AssemblyContext).FullName!, throwOnError: true)!;
        }

        public Type Type { get; }

        public Func<Assembly, bool> AssemblyFilter
        {
            get => Get<Func<Assembly, bool>>(nameof(AssemblyContext.AssemblyFilterCallback));
            set => Set(nameof(AssemblyContext.AssemblyFilterCallback), value);
        }

        public Func<Assembly, IEnumerable<Assembly>> Dependencies
        {
            get => Get<Func<Assembly, IEnumerable<Assembly>>>(nameof(AssemblyContext.AssemblyDependenciesCallback));
            set => Set(nameof(AssemblyContext.AssemblyDependenciesCallback), value);
        }

        public Func<AssemblyName, bool> ReferenceFilter
        {
            get => Get<Func<AssemblyName, bool>>(nameof(AssemblyContext.AssemblyDependenciesFilterCallback));
            set => Set(nameof(AssemblyContext.AssemblyDependenciesFilterCallback), value);
        }

        public IReadOnlyList<Assembly> CurrentDomainAssemblies => Get<IReadOnlyList<Assembly>>(nameof(AssemblyContext.CurrentDomainAssemblies));

        private T Get<T>(string name) => (T)Type.GetProperty(name)!.GetValue(null)!;

        private void Set(string name, object value) => Type.GetProperty(name)!.SetValue(null, value);

        public void Dispose() => _loadContext.Unload();
    }
}
