---
description: 'Writing Unit Tests in Savvyio'
applyTo: '**/*.cs'
---

# Writing Unit Tests in Savvyio

This document provides instructions for writing unit tests in the Savvyio codebase. Please follow these guidelines to ensure consistency and maintainability.

---

## 1. Base Class

**Always inherit from the `Test` base class** for all unit test classes.  
This ensures consistent setup, teardown, and output handling across all tests.

```csharp
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Your.Namespace
{
    public class YourTestClass : Test
    {
        public YourTestClass(ITestOutputHelper output) : base(output)
        {
        }

        // Your tests here
    }
}
```

---

## 2. Test Method Attributes

- Use `[Fact]` for standard unit tests.
- Use `[Theory]` with `[InlineData]` or other data sources for parameterized tests.

---

## 3. Naming Conventions

- **Test classes**: End with `Test` (e.g., `CommandDispatcherTest`).
- **Test methods**: Use descriptive names that state the expected behavior (e.g., `ShouldReturnTrue_WhenConditionIsMet`).

---

## 4. Assertions

- Use `Assert` methods from xUnit for all assertions.
- Prefer explicit and expressive assertions (e.g., `Assert.Equal`, `Assert.NotNull`, `Assert.Contains`).

---

## 5. File and Namespace Organization

- Place test files in the appropriate test project and folder structure.
- Use namespaces that mirror the source code structure.
- The unit tests for the Savvyio.Foo assembly live in the Savvyio.Foo.Tests assembly.
- The functional tests for the Savvyio.Foo assembly live in the Savvyio.Foo.FunctionalTests assembly.
- Test class names end with Test and live in the same namespace as the class being tested, e.g., the unit tests for the Boo class that resides in the Savvyio.Foo assembly would be named BooTest and placed in the Savvyio.Foo namespace in the Savvyio.Foo.Tests assembly.
- Modify the associated .csproj file to override the root namespace, e.g., <RootNamespace>Savvyio.Foo</RootNamespace>.

---

## 6. Example Test

```csharp
using Codebelt.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Commands
{
    /// <summary>
    /// Tests for the <see cref="DefaultCommand"/> class.
    /// </summary>
    public class CommandTest : Test
    {
        public CommandTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DefaultCommand_Ensure_Initialization_Defaults()
        {
            var sut = new DefaultCommand();

            Assert.IsAssignableFrom<Command>(sut);
            Assert.IsAssignableFrom<ICommand>(sut);
            Assert.IsAssignableFrom<Request>(sut);
            Assert.IsAssignableFrom<IRequest>(sut);
            Assert.IsAssignableFrom<IMetadata>(sut);
            Assert.Contains(sut.Metadata, pair => pair.Key == MetadataDictionary.CorrelationId);
        }
    }
}
```

---

## 7. Additional Guidelines

- Keep tests focused and isolated.
- Do not rely on external systems except for xUnit itself and Codebelt.Extensions.Xunit (and derived from this).
- Ensure tests are deterministic and repeatable.

---

For further examples, refer to existing test files such as  
[`test/Savvyio.Commands.Tests/CommandDispatcherTest.cs`](test/Savvyio.Commands.Tests/CommandDispatcherTest.cs)  
and  
[`test/Savvyio.Commands.Tests/CommandTest.cs`](test/Savvyio.Commands.Tests/CommandTest.cs).

---
description: 'Writing XML documentation in Savvyio'
applyTo: '**/*.cs'
---

# Writing XML documentation in Savvyio

This document provides instructions for writing XML documentation.

---

## 1. Documentation Style

- Use the same documentation style as found throughout the codebase.
- Add XML doc comments to public and protected classes and methods where appropriate.
- Example:

```csharp
using Cuemon.Extensions.DependencyInjection;
using Cuemon.Extensions.Text.Json.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;
using Cuemon;
using Savvyio.Extensions.Text.Json;

namespace Savvyio.Extensions.DependencyInjection.Text.Json
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds an <see cref="JsonMarshaller" /> implementation to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend.</param>
        /// <param name="jsonSetup">The <see cref="JsonFormatterOptions" /> which may be configured. Default is optimized for messaging.</param>
        /// <param name="serviceSetup">The <see cref="ServiceOptions" /> which may be configured. Default is <see cref="ServiceLifetime.Singleton"/>.</param>
        /// <returns>A reference to <paramref name="services"/> so that additional calls can be chained.</returns>
        /// <remarks>The implementation will be type forwarded accordingly.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> cannot be null.
        /// </exception>
        public static IServiceCollection AddJsonMarshaller(this IServiceCollection services, Action<JsonFormatterOptions> jsonSetup = null, Action<ServiceOptions> serviceSetup = null)
        {
            Validator.ThrowIfNull(services);
            return services
                .AddMarshaller<JsonMarshaller>(serviceSetup)
                .AddConfiguredOptions(jsonSetup ?? (o => o.Settings.WriteIndented = false));
        }
    }
}


using System;
using Cuemon;
using Cuemon.Configuration;

namespace Savvyio.Extensions.NATS
{
    /// <summary>
    /// Configuration options that is related to NATS.
    /// </summary>
    /// <seealso cref="IValidatableParameterObject" />
    public class NatsMessageOptions : IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NatsMessageOptions"/> class with default values.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="NatsMessageOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="NatsUrl"/></term>
        ///         <description><c>new Uri("nats://127.0.0.1:4222")</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Subject"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public NatsMessageOptions()
        {
            NatsUrl = new Uri("nats://127.0.0.1:4222");
        }

        /// <summary>
        /// Gets or sets the URI of the NATS server.
        /// </summary>
        public Uri NatsUrl { get; set; }

        /// <summary>
        /// Gets or sets the subject to publish or subscribe to in NATS.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Validates the current options and throws an exception if the state is invalid.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Subject"/> is null or whitespace - or -
        /// <see cref="NatsUrl"/> is null.
        /// </exception>
        public virtual void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(NatsUrl == null);
            Validator.ThrowIfInvalidState(string.IsNullOrWhiteSpace(Subject));
        }
    }
}

```