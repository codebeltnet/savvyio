#nullable enable
using System;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio;
using Xunit;

namespace Cuemon.Reflection;

public class AssemblyContextTest : Test
{
    public AssemblyContextTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldReturnNonEmptyList()
    {
        var assemblies = AssemblyContext.GetCurrentDomainAssemblies();

        Assert.NotNull(assemblies);
        Assert.NotEmpty(assemblies);
        Assert.Contains(typeof(AssemblyContextTest).Assembly, assemblies);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldExcludeCuemonCoreByDefault()
    {
        Assert.DoesNotContain(typeof(AssemblyContext).Assembly, AssemblyContext.GetCurrentDomainAssemblies());
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldExcludeSavvyioCore_WhenConfigured()
    {
        var savvyioCore = typeof(SavvyioOptions).Assembly;

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == savvyioCore;
            options.ExcludedAssemblies.Add(savvyioCore);
        });

        Assert.DoesNotContain(savvyioCore, assemblies);
        Assert.DoesNotContain(typeof(AssemblyContext).Assembly, assemblies);
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldNotContainSystemOrMicrosoftAssemblies()
    {
        Assert.All(AssemblyContext.GetCurrentDomainAssemblies(), assembly =>
        {
            Assert.False(assembly.FullName?.StartsWith(nameof(System), StringComparison.Ordinal),
                $"Assembly '{assembly.FullName}' should have been filtered out.");
            Assert.False(assembly.FullName?.StartsWith(nameof(Microsoft), StringComparison.Ordinal),
                $"Assembly '{assembly.FullName}' should have been filtered out.");
        });
    }

    [Fact]
    public void AssemblyContextOptions_ShouldHaveValidDefaults()
    {
        var options = new AssemblyContextOptions();

        Assert.True(options.IncludeReferencedAssemblies);
        Assert.NotNull(options.AssemblyFilter);
        Assert.NotNull(options.ReferencedAssemblyFilter);
        Assert.Equal(new[] { typeof(AssemblyContext).Assembly }, options.ExcludedAssemblies);
        options.ValidateOptions();
    }

    [Fact]
    public void AssemblyFilter_ShouldAcceptCustomDelegate()
    {
        var options = new AssemblyContextOptions();
        Func<Assembly, bool> custom = _ => true;

        options.AssemblyFilter = custom;

        Assert.Same(custom, options.AssemblyFilter);
        options.ValidateOptions();
    }

    [Fact]
    public void ReferencedAssemblyFilter_ShouldAcceptCustomDelegate()
    {
        var options = new AssemblyContextOptions();
        Func<AssemblyName, bool> custom = _ => true;

        options.ReferencedAssemblyFilter = custom;

        Assert.Same(custom, options.ReferencedAssemblyFilter);
        options.ValidateOptions();
    }

    [Fact]
    public void AssemblyContextOptions_ShouldKeepConfigurationLocalToEachInstance()
    {
        var first = new AssemblyContextOptions
        {
            AssemblyFilter = _ => false,
            ReferencedAssemblyFilter = _ => false,
            IncludeReferencedAssemblies = false
        };
        first.ExcludedAssemblies.Clear();
        var second = new AssemblyContextOptions();

        Assert.NotSame(first.AssemblyFilter, second.AssemblyFilter);
        Assert.NotSame(first.ReferencedAssemblyFilter, second.ReferencedAssemblyFilter);
        Assert.True(second.IncludeReferencedAssemblies);
        Assert.Equal(new[] { typeof(AssemblyContext).Assembly }, second.ExcludedAssemblies);
    }

    [Theory]
    [InlineData(nameof(AssemblyContextOptions.AssemblyFilter))]
    [InlineData(nameof(AssemblyContextOptions.ReferencedAssemblyFilter))]
    [InlineData(nameof(AssemblyContextOptions.ExcludedAssemblies))]
    public void ValidateOptions_ShouldRejectNullConfiguration(string propertyName)
    {
        var options = new AssemblyContextOptions();
        typeof(AssemblyContextOptions).GetProperty(propertyName)!.SetValue(options, null);

        var error = Assert.Throws<InvalidOperationException>(options.ValidateOptions);

        Assert.Contains(propertyName, error.Message);
    }

    [Theory]
    [InlineData(nameof(AssemblyContextOptions.AssemblyFilter))]
    [InlineData(nameof(AssemblyContextOptions.ReferencedAssemblyFilter))]
    [InlineData(nameof(AssemblyContextOptions.ExcludedAssemblies))]
    public void GetCurrentDomainAssemblies_ShouldRejectInvalidConfiguration(string propertyName)
    {
        var error = Assert.Throws<ArgumentException>(() => AssemblyContext.GetCurrentDomainAssemblies(options =>
            typeof(AssemblyContextOptions).GetProperty(propertyName)!.SetValue(options, null)));

        Assert.Equal("setup", error.ParamName);
        Assert.Contains(propertyName, Assert.IsType<InvalidOperationException>(error.InnerException).Message);
    }

    [Fact]
    public void AssemblyFilter_ShouldIncludeSavvyioAssembly()
    {
        Assert.True(new AssemblyContextOptions().AssemblyFilter(typeof(SavvyioOptions).Assembly));
    }

    [Fact]
    public void AssemblyFilter_ShouldExcludeSystemAssembly()
    {
        Assert.False(new AssemblyContextOptions().AssemblyFilter(typeof(string).Assembly));
    }

    [Fact]
    public void ReferencedAssemblyFilter_ShouldIncludeSavvyioAssemblyName()
    {
        Assert.True(new AssemblyContextOptions().ReferencedAssemblyFilter(typeof(SavvyioOptions).Assembly.GetName()));
    }

    [Fact]
    public void ReferencedAssemblyFilter_ShouldExcludeSystemAssemblyName()
    {
        Assert.False(new AssemblyContextOptions().ReferencedAssemblyFilter(typeof(string).Assembly.GetName()));
    }

    [Fact]
    public void GetCurrentDomainAssemblies_ShouldIncludeRoot_WhenAllReferencesAreFilteredOut()
    {
        var root = typeof(SavvyioOptions).Assembly;

        var assemblies = AssemblyContext.GetCurrentDomainAssemblies(options =>
        {
            options.AssemblyFilter = assembly => assembly == root;
            options.ReferencedAssemblyFilter = _ => false;
        });

        Assert.Same(root, Assert.Single(assemblies));
    }
}
