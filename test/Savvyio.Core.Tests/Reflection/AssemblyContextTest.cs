using System;
using System.Linq;
using System.Reflection;
using Codebelt.Extensions.Xunit;
using Savvyio.Reflection;
using Xunit;

namespace Savvyio.Reflection
{
    public class AssemblyContextTest : Test
    {
        public AssemblyContextTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CurrentDomainAssemblies_ShouldReturnNonEmptyList()
        {
            var assemblies = AssemblyContext.CurrentDomainAssemblies;

            Assert.NotNull(assemblies);
            Assert.NotEmpty(assemblies);
        }

        [Fact]
        public void CurrentDomainAssemblies_ShouldNotContainSavvyioCoreAssembly()
        {
            var savvyioCoreAssembly = typeof(AssemblyContext).Assembly;

            Assert.DoesNotContain(savvyioCoreAssembly, AssemblyContext.CurrentDomainAssemblies);
        }

        [Fact]
        public void CurrentDomainAssemblies_ShouldNotContainSystemOrMicrosoftAssemblies()
        {
            foreach (var assembly in AssemblyContext.CurrentDomainAssemblies)
            {
                Assert.False(assembly.FullName?.StartsWith(nameof(System), StringComparison.Ordinal),
                    $"Assembly '{assembly.FullName}' should have been filtered out.");
                Assert.False(assembly.FullName?.StartsWith(nameof(Microsoft), StringComparison.Ordinal),
                    $"Assembly '{assembly.FullName}' should have been filtered out.");
            }
        }

        [Fact]
        public void AssemblyFilterCallback_ShouldReturnDefaultNonNullDelegate()
        {
            var callback = AssemblyContext.AssemblyFilterCallback;

            Assert.NotNull(callback);
        }

        [Fact]
        public void AssemblyFilterCallback_ShouldAcceptCustomDelegate()
        {
            var original = AssemblyContext.AssemblyFilterCallback;
            try
            {
                Func<Assembly, bool> custom = _ => true;
                AssemblyContext.AssemblyFilterCallback = custom;

                Assert.Same(custom, AssemblyContext.AssemblyFilterCallback);
            }
            finally
            {
                AssemblyContext.AssemblyFilterCallback = original;
            }
        }

        [Fact]
        public void AssemblyFilterCallback_ShouldThrowArgumentNullException_WhenAssignedNull()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyContext.AssemblyFilterCallback = null);
        }

        [Fact]
        public void AssemblyDependenciesCallback_ShouldReturnDefaultNonNullDelegate()
        {
            var callback = AssemblyContext.AssemblyDependenciesCallback;

            Assert.NotNull(callback);
        }

        [Fact]
        public void AssemblyDependenciesCallback_ShouldAcceptCustomDelegate()
        {
            var original = AssemblyContext.AssemblyDependenciesCallback;
            try
            {
                Func<Assembly, System.Collections.Generic.IEnumerable<Assembly>> custom = a => Enumerable.Repeat(a, 1);
                AssemblyContext.AssemblyDependenciesCallback = custom;

                Assert.Same(custom, AssemblyContext.AssemblyDependenciesCallback);
            }
            finally
            {
                AssemblyContext.AssemblyDependenciesCallback = original;
            }
        }

        [Fact]
        public void AssemblyDependenciesCallback_ShouldThrowArgumentNullException_WhenAssignedNull()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyContext.AssemblyDependenciesCallback = null);
        }

        [Fact]
        public void AssemblyDependenciesFilterCallback_ShouldReturnDefaultNonNullDelegate()
        {
            var callback = AssemblyContext.AssemblyDependenciesFilterCallback;

            Assert.NotNull(callback);
        }

        [Fact]
        public void AssemblyDependenciesFilterCallback_ShouldAcceptCustomDelegate()
        {
            var original = AssemblyContext.AssemblyDependenciesFilterCallback;
            try
            {
                Func<AssemblyName, bool> custom = _ => true;
                AssemblyContext.AssemblyDependenciesFilterCallback = custom;

                Assert.Same(custom, AssemblyContext.AssemblyDependenciesFilterCallback);
            }
            finally
            {
                AssemblyContext.AssemblyDependenciesFilterCallback = original;
            }
        }

        [Fact]
        public void AssemblyDependenciesFilterCallback_ShouldThrowArgumentNullException_WhenAssignedNull()
        {
            Assert.Throws<ArgumentNullException>(() => AssemblyContext.AssemblyDependenciesFilterCallback = null);
        }

        [Fact]
        public void AssemblyFilterCallback_DefaultFilter_ShouldIncludeNonSystemAssembly()
        {
            var callback = AssemblyContext.AssemblyFilterCallback;
            var savvyioAssembly = typeof(AssemblyContext).Assembly;

            Assert.True(callback(savvyioAssembly));
        }

        [Fact]
        public void AssemblyFilterCallback_DefaultFilter_ShouldExcludeSystemAssembly()
        {
            var callback = AssemblyContext.AssemblyFilterCallback;
            var systemAssembly = typeof(string).Assembly;

            Assert.False(callback(systemAssembly));
        }

        [Fact]
        public void AssemblyDependenciesFilterCallback_DefaultFilter_ShouldIncludeNonSystemAssemblyName()
        {
            var callback = AssemblyContext.AssemblyDependenciesFilterCallback;
            var assemblyName = typeof(AssemblyContext).Assembly.GetName();

            Assert.True(callback(assemblyName));
        }

        [Fact]
        public void AssemblyDependenciesFilterCallback_DefaultFilter_ShouldExcludeSystemAssemblyName()
        {
            var callback = AssemblyContext.AssemblyDependenciesFilterCallback;
            var assemblyName = typeof(string).Assembly.GetName();

            Assert.False(callback(assemblyName));
        }

        [Fact]
        public void AssemblyDependenciesCallback_DefaultCallback_ShouldYieldAtLeastTheInputAssembly()
        {
            var callback = AssemblyContext.AssemblyDependenciesCallback;
            var assembly = typeof(AssemblyContext).Assembly;

            var result = callback(assembly).ToList();

            Assert.Contains(assembly, result);
        }
    }
}
