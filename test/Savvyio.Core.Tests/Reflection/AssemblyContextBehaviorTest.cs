#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime.Loader;
using Codebelt.Extensions.Xunit;
using Cuemon;
using Savvyio;
using Savvyio.Commands;
using Xunit;

namespace Cuemon.Reflection;

public class AssemblyContextBehaviorTest : Test
{
    public AssemblyContextBehaviorTest(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("System.Application", false)]
    [InlineData("Microsoft.Application", false)]
    [InlineData("Application.System", true)]
    [InlineData("system.Application", true)]
    [InlineData("microsoft.Application", true)]
    public void AssemblyFilter_ShouldUseOrdinalPrefixFiltering(string? fullName, bool expected)
    {
        Assert.Equal(expected, new AssemblyContextOptions().AssemblyFilter(new ExportedTypesAssembly(fullName)));
    }

    [Fact]
    public void AssemblyFilter_ShouldRejectNullAssembly()
    {
        Assert.False(new AssemblyContextOptions().AssemblyFilter(null!));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("System.Application", false)]
    [InlineData("Microsoft.Application", false)]
    [InlineData("Application.System", true)]
    [InlineData("system.Application", true)]
    [InlineData("microsoft.Application", true)]
    public void ReferencedAssemblyFilter_ShouldUseOrdinalPrefixFiltering(string? name, bool expected)
    {
        var assemblyName = name is null ? null : new AssemblyName(name);

        Assert.Equal(expected, new AssemblyContextOptions().ReferencedAssemblyFilter(assemblyName!));
    }

    [Theory]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException), false)]
    [InlineData(typeof(SavvyioOptions), true)]
    public void AssemblyFilter_ShouldInspectExportedRootNamespaces(Type exportedType, bool expected)
    {
        var assembly = new ExportedTypesAssembly($"Application.NamespaceFilter.{exportedType.FullName}", () => [exportedType]);

        Assert.Equal(expected, new AssemblyContextOptions().AssemblyFilter(assembly));
    }

    [Theory]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(SavvyioOptions), true)]
    public void AssemblyFilter_ShouldInspectLoadableTypes_WhenSomeExportedTypesCannotLoad(Type exportedType, bool expected)
    {
        var assembly = new ExportedTypesAssembly($"Application.PartialLoad.{exportedType.FullName}", () =>
            throw new ReflectionTypeLoadException([null, exportedType], [new TypeLoadException()]));

        Assert.Equal(expected, new AssemblyContextOptions().AssemblyFilter(assembly));
    }

    [Fact]
    public void AssemblyFilter_ShouldAllowAssembly_WhenExportedTypeScanHasRecoverableFailure()
    {
        var assembly = new ExportedTypesAssembly("Application.RecoverableTypeScanFailure", () => throw new NotSupportedException());

        Assert.True(new AssemblyContextOptions().AssemblyFilter(assembly));
    }

    [Fact]
    public void AssemblyFilter_ShouldAllowAssembly_WhenExportedTypesAreUnavailable()
    {
        var assembly = new ExportedTypesAssembly("Application.UnavailableExportedTypes", () => null!);

        Assert.True(new AssemblyContextOptions().AssemblyFilter(assembly));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldTraverseTransitiveReferences()
    {
        var root = typeof(Command).Assembly;
        var core = typeof(SavvyioOptions).Assembly;
        var transitive = typeof(AssemblyContext).Assembly;
        // Commands references Savvyio.Core, which in turn references Cuemon.Core.
        Assert.DoesNotContain(root.GetReferencedAssemblies(), name => name.FullName == transitive.FullName);

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.ExcludedAssemblies.Clear();
        });

        Assert.Same(root, assemblies[0]);
        Assert.Contains(core, assemblies);
        Assert.Contains(transitive, assemblies);
        Assert.Equal(assemblies.Count, assemblies.Distinct().Count());
        Assert.DoesNotContain(assemblies, assembly => assembly.FullName!.StartsWith("System", StringComparison.Ordinal));
        Assert.DoesNotContain(assemblies, assembly => assembly.FullName!.StartsWith("Microsoft", StringComparison.Ordinal));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldSkipSelfReferencesAndDuplicates()
    {
        var dependency = typeof(Validator).Assembly;
        var rootName = new AssemblyName("Savvyio.Tests.AssemblyContext.SelfReferences") { Version = new Version(1, 0, 0, 0) };
        using var fixture = new ReferencedAssemblyFixture(rootName, rootName, dependency.GetName(), dependency.GetName());
        var root = fixture.Assembly;
        var inspected = new List<AssemblyName>();
        Assert.Equal(3, root.GetReferencedAssemblies().Length);
        Assert.Equal(root.FullName, root.GetReferencedAssemblies()[0].FullName);

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.ReferencedAssemblyFilter = name =>
            {
                inspected.Add(name);
                return name.FullName == root.FullName || name.FullName == dependency.FullName;
            };
        });

        Assert.Equal(new[] { root, dependency }, assemblies);
        Assert.Equal(root.GetReferencedAssemblies().Concat(dependency.GetReferencedAssemblies()).Select(name => name.FullName),
            inspected.Select(name => name.FullName));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldContinueAfterUnresolvableReference()
    {
        var dependency = typeof(Validator).Assembly;
        var missing = new AssemblyName("Savvyio.Tests.NonexistentDependency.8d9b24a2");
        using var fixture = new ReferencedAssemblyFixture(new AssemblyName("Savvyio.Tests.AssemblyContext.MissingReference"), missing, dependency.GetName());
        var root = fixture.Assembly;

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.ReferencedAssemblyFilter = name => name.Name == missing.Name || name.FullName == dependency.FullName;
        });

        Assert.Equal(new[] { root, dependency }, assemblies);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldApplyReferenceFilterWithoutFilteringRoot()
    {
        var root = typeof(Command).Assembly;
        var inspected = new List<AssemblyName>();

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.ReferencedAssemblyFilter = name =>
            {
                inspected.Add(name);
                return false;
            };
        });

        Assert.Same(root, Assert.Single(assemblies));
        Assert.Equal(root.GetReferencedAssemblies().Select(name => name.FullName), inspected.Select(name => name.FullName));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldFilterExpandDeduplicateAndExcludeInfrastructureAssemblies()
    {
        var dependency = typeof(Command).Assembly;
        var savvyioCore = typeof(SavvyioOptions).Assembly;
        var cuemonCore = typeof(AssemblyContext).Assembly;
        using var fixture = new ReferencedAssemblyFixture(new AssemblyName("Savvyio.Tests.AssemblyContext.SharedReferences"),
            dependency.GetName(), dependency.GetName(), savvyioCore.GetName(), cuemonCore.GetName());
        var root = fixture.Assembly;
        var expanded = new List<Assembly>();

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly =>
            {
                if (assembly != root && assembly != dependency) { return false; }
                expanded.Add(assembly);
                return true;
            };
            options.ReferencedAssemblyFilter = name => name.FullName == dependency.FullName ||
                                                      name.FullName == savvyioCore.FullName || name.FullName == cuemonCore.FullName;
            options.ExcludedAssemblies.Add(savvyioCore);
        });

        Assert.Equal(2, expanded.Count);
        Assert.Contains(root, expanded);
        Assert.Contains(dependency, expanded);
        Assert.Equal(2, assemblies.Count);
        Assert.Contains(root, assemblies);
        Assert.Contains(dependency, assemblies);
        Assert.DoesNotContain(savvyioCore, assemblies);
        Assert.DoesNotContain(cuemonCore, assemblies);
        Assert.Throws<NotSupportedException>(() => ((IList<Assembly>)assemblies).Add(root));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldSkipTraversal_WhenReferencesAreDisabled()
    {
        var root = typeof(Command).Assembly;

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.IncludeReferencedAssemblies = false;
            options.ReferencedAssemblyFilter = _ => throw new InvalidOperationException("Must not traverse references.");
        });

        Assert.Same(root, Assert.Single(assemblies));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldUseFreshOptionsAndResultsForEachCall()
    {
        var firstRoot = typeof(Command).Assembly;
        var secondRoot = typeof(SavvyioOptions).Assembly;
        var first = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == firstRoot;
            options.IncludeReferencedAssemblies = false;
            options.ExcludedAssemblies.Add(secondRoot);
        });

        var second = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == secondRoot;
            options.ReferencedAssemblyFilter = _ => false;
        });

        Assert.NotSame(first, second);
        Assert.Same(firstRoot, Assert.Single(first));
        Assert.Same(secondRoot, Assert.Single(second));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldAllowEmptyResult()
    {
        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = _ => false;
            options.ReferencedAssemblyFilter = _ => throw new InvalidOperationException("No assembly passed the filter.");
        });

        Assert.Empty(assemblies);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetCurrentDomainAssemblies_ShouldPropagateFilterFailureWithoutCachingIt(bool failReferenceFilter)
    {
        var root = typeof(Command).Assembly;
        var expected = new InvalidOperationException("Assembly filter failed.");

        var error = Assert.Throws<InvalidOperationException>(() => AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => failReferenceFilter ? assembly == root : throw expected;
            options.ReferencedAssemblyFilter = _ => throw expected;
        }));
        var next = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.IncludeReferencedAssemblies = false;
        });

        Assert.Same(expected, error);
        Assert.Same(root, Assert.Single(next));
    }

    private sealed class ExportedTypesAssembly(string? fullName, Func<Type[]>? exportedTypes = null) : Assembly
    {
        public override string? FullName => fullName;

        public override Type[] GetExportedTypes() => exportedTypes is null ? [typeof(SavvyioOptions)] : exportedTypes();
    }

    // Real in-memory metadata lets the public scanner traverse self, duplicate and missing
    // references without invoking private helpers or altering global configuration.
    private sealed class ReferencedAssemblyFixture : IDisposable
    {
        private readonly AssemblyLoadContext _loadContext = new(null, isCollectible: true);

        public ReferencedAssemblyFixture(AssemblyName name, params AssemblyName[] references)
        {
            var metadata = new MetadataBuilder();
            metadata.AddModule(0, metadata.GetOrAddString(name.Name!), default, default, default);
            metadata.AddAssembly(metadata.GetOrAddString(name.Name!), name.Version ?? new Version(1, 0, 0, 0),
                default, default, default, default);
            metadata.AddTypeDefinition(TypeAttributes.NotPublic, default, metadata.GetOrAddString("<Module>"),
                default, MetadataTokens.FieldDefinitionHandle(1), MetadataTokens.MethodDefinitionHandle(1));
            foreach (var reference in references)
            {
                metadata.AddAssemblyReference(metadata.GetOrAddString(reference.Name!), reference.Version ?? new Version(1, 0, 0, 0),
                    metadata.GetOrAddString(reference.CultureName ?? ""), metadata.GetOrAddBlob(reference.GetPublicKeyToken() ?? []),
                    default, default);
            }
            var image = new BlobBuilder();
            new ManagedPEBuilder(new PEHeaderBuilder(imageCharacteristics: Characteristics.Dll),
                new MetadataRootBuilder(metadata), new BlobBuilder()).Serialize(image);
            using var stream = new MemoryStream(image.ToArray());
            Assembly = _loadContext.LoadFromStream(stream);
        }

        public Assembly Assembly { get; }

        public void Dispose() => _loadContext.Unload();
    }
}
