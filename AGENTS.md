# Agent Instructions for Savvy I/O

This document provides guidance for AI agents working in the Savvy I/O repository.

## Project Overview

**Savvy I/O** is an open-source .NET class library (MIT license) providing a SOLID and clean foundation for writing DDD (Domain-Driven Design), CQRS (Command Query Responsibility Segregation), and Event Sourcing applications. It is designed to be intuitive and removes the complexity normally associated with these architectural patterns.

The solution targets .NET 10.0 and .NET 9.0. It comprises:

- **Core Libraries** — DDD, CQRS, Commands, Queries, Event-Driven, and Domain Event Sourcing
- **Data Access Extensions** — Support for Dapper, Entity Framework Core
- **Messaging Extensions** — Support for AWS SNS/SQS, Azure Queue Storage/Event Grid, RabbitMQ, NATS
- **Marshalling Extensions** — Support for System.Text.Json and Newtonsoft.Json
- **Dependency Injection Extensions** — Microsoft DI integration for all extensions
- **Savvyio.App** — Productivity package bundling common default APIs for complete DDD, CQRS, and Event Sourcing applications

## Core Architectural Patterns

Savvy I/O is built around these fundamental patterns:

- **Domain-Driven Design (DDD)** — Aggregate roots, value objects, domain services, repositories, and domain events
- **CQRS (Command Query Responsibility Segregation)** — Separate read and write models, command/query handlers, dispatchers
- **Event Sourcing** — Immutable event store, event handlers, event reconstruction, temporal consistency
- **Dependency Injection** — Microsoft DI container integration for all extensions and features

When working on Savvy I/O features:
- Maintain clear boundaries between domain models, application services, and infrastructure
- Follow CQRS principles: commands modify state, queries retrieve state without side effects
- Design aggregates with explicit boundaries and consistency rules
- Use domain events to communicate between aggregates and enable eventual consistency
- Keep extension APIs consistent with the core library's conventions

## Coding Standards

- **Text encoding:** UTF-8 for text files (enforced via `.editorconfig`)
- **Template rewrites:** Preserve UTF-8 explicitly when scripts or tools rewrite text files; avoid locale-dependent encoding defaults
- **Namespaces:** File-scoped namespaces are required (enforced via `.editorconfig`)
- **Top-level statements:** Not allowed (enforced via `.editorconfig`)
- **Language version:** Always use the latest C# features (`LangVersion=latest`)
- **Nullable:** Enable nullable reference types in all new code
- **XML documentation:** All public APIs must have XML documentation comments
- **Testing:** Use xUnit v3 with Codebelt.Extensions.Xunit base classes

## Project Structure

- `src/` — Production source code
  - **Core** — `Savvyio.Core/`, `Savvyio.App/`
  - **DDD & Commands** — `Savvyio.Domain/`, `Savvyio.Domain.EventSourcing/`, `Savvyio.Commands/`, `Savvyio.Commands.Messaging/`
  - **CQRS & Events** — `Savvyio.Queries/`, `Savvyio.EventDriven/`, `Savvyio.EventDriven.Messaging/`, `Savvyio.Messaging/`
  - **Data Access** — `Savvyio.Extensions.Dapper/`, `Savvyio.Extensions.DapperExtensions/`, `Savvyio.Extensions.EFCore/`, `Savvyio.Extensions.EFCore.Domain/`, `Savvyio.Extensions.EFCore.Domain.EventSourcing/`
  - **Marshalling** — `Savvyio.Extensions.Newtonsoft.Json/`, `Savvyio.Extensions.Text.Json/`
  - **Messaging** — `Savvyio.Extensions.NATS/`, `Savvyio.Extensions.RabbitMQ/`, `Savvyio.Extensions.SimpleQueueService/`, `Savvyio.Extensions.QueueStorage/`
  - **Dependency Injection** — `Savvyio.Extensions.DependencyInjection/` and integration variants for Dapper, EFCore, Marshalling, and Messaging
  - **Utilities** — `Savvyio.Extensions.Dispatchers/`
- `test/` — Unit and integration tests (project names end with `Tests`)
- `.nuget/` — Per-package NuGet metadata (icon, README, release notes)
- `.docfx/` — DocFX documentation configuration
- `.github/` — CI/CD workflows and contributing guidelines
- Dockerfiles and compose — Test environment setup (LocalStack, test isolation)

## Test Conventions

- Test project names must end with `Tests` (e.g., `{PROJECT_NAME}.Tests`)
- Test classes should inherit from the `Test` base class in `Codebelt.Extensions.Xunit`
- Use `Microsoft.Testing.Platform` as the test runner (`UseMicrosoftTestingPlatformRunner=true`)
- All tests are executable (`OutputType=Exe`)
- Test namespaces must match the SUT namespace (System Under Test), without `.Tests` suffix
- See the detailed test writing guidelines in this document

## Build & CI

- Centralized package versions via `Directory.Packages.props`
- Resolve new or updated `Directory.Packages.props` versions from NuGet.org and keep them on the latest stable listed releases
- Centralized build configuration via `Directory.Build.props`
- MinVer for semantic versioning from Git tags
- Strong-name signing is enabled in CI environments (`CI=true`)
- Keep `.github/dependabot.yml` enabled at the repo root so central NuGet package management stays current

## .bot/ Folder

If a `.bot/` folder exists at the root, it contains **confidential, local-only** working material for AI agents — product requirement documents (PRDs), design proposals, agentic loop state, and brainstorming outputs. This folder is gitignored and never committed.

When starting creative or design work (new features, architecture decisions, PRD drafts), use the [brainstorming skill](https://skills.sh/obra/superpowers/brainstorming) and save outputs to `.bot/`. Only move finalized, non-confidential instructions into `AGENTS.md`.

## Git Operations Safeguards

Agents must never automatically commit code changes or push to remote repositories. Both actions require explicit user approval:

- **Commits**: Always request confirmation from the user before staging and committing code. Present a clear summary of the changes and wait for approval before executing the commit.
- **Remote Operations**: Do not push, pull, fetch, or interact with `origin` or any remote repository without explicit user instruction. These operations modify repository history and can cause data loss if performed unexpectedly.

**Rationale:** Automatic commits can clutter history with incomplete work, temporary debugging code, or unintended changes. Unexpected remote operations risk overwriting or losing commits on shared branches. Always require explicit user approval before performing these actions.

<!-- dotnet-docfx-digest:start -->
## DocFX Documentation Maintenance

When changing public .NET APIs, keep the DocFX documentation current in the same change set.

Documentation updates must cover public API only. Do not document private or internal types or members. Do not create namespace overview pages for namespaces that contain no public API.

Public non-abstraction types — including enums, structs, records, plain classes, and static extension containers — are valid documentation targets. Generic public types and generic extension methods are valid documentation targets too. Do not exclude a type solely because it is generic or because reflection reports it as abstract and sealed (that is the IL pattern for a static class).

For public non-abstraction types, include at least one realistic, copy/paste-ready usage example on the generated type page/overwrite section for that type UID. For example, a public `Class1` requires an example on the `Class1` API page, not only on the namespace page. Prefer deriving examples from existing unit, functional, or integration tests, but convert test code into real-life consumer-oriented usage.

Missing type examples must be added through per-type DocFX overwrite files under `.docfx/api/types/{TypeUid}.md` in Codebelt repositories. Namespace overview text and `Extension Members` tables are not substitutes for type-page examples.

Public extension methods must have examples too. Listing an extension method in an `Extension Members` table is required, but it is not enough.

All added or changed code samples must be deterministic and verified to compile. Do not add pseudo-code, ellipses, hidden test helpers, or examples that rely on unverified behavior.

Compilation is necessary but not sufficient. Do not present runtime implementation names such as `services.GetType().Name` or `host.GetType().FullName` as the example outcome. Show application behavior, configured state, a resolved domain service, an HTTP response, or another result that explains why a caller uses the API. Application-entry-point examples must not declare an empty local `Program` type merely to compile; show a real entry point or clearly identify the referenced application project.

Every namespace containing public API must have a DocFX namespace overview page named after the namespace, such as `X.Y.Z.md`, under `.docfx/api/namespaces/`, using DocFX overwrite front matter with the namespace `uid`.

Namespace pages must identify key entry points from release notes, package documentation, public factories/builders, and strong functional tests, then help readers choose among adjacent workflows. When the package complements a well-known upstream API, compare concrete acquisition, customization, lifecycle, and sharing tradeoffs from current official guidance; do not claim drop-in replacement compatibility without evidence.

Namespaces exposing public extension methods must document those extension members at namespace level. The namespace page must include an `Extension Members` table listing the extended type, the extension marker, and the public extension methods. Extension members are rendered under the heading `Extension Members`.

Both namespace overwrite files and type overwrite files are required deliverables in the same run. Generating only namespace pages or only type pages is incomplete.

`docfx.json` must keep namespace and type overwrite files in separate subdirectories. `build.overwrite` must include both `api/namespaces/**/*.md` (for namespace pages) and `api/types/**/*.md` (for type pages). `build.content` must exclude both `api/namespaces/**` and `api/types/**` to prevent overwrite Markdown from being treated as conceptual content. Do not use `api/**/*.md` under `build.overwrite` or `build.content`.

Availability must be documented by referencing the appropriate include file when one exists, or by adding explicit availability text when no suitable include exists. Availability must reflect the actual target frameworks, conditional compilation, and project configuration.

For conditionally compiled APIs, choose the executable test framework from the asset that contains the API. Inspect the preprocessor condition, project TFMs, package `lib/` assets, and resolved consumer asset before changing a sample. For APIs under `NETSTANDARD2_0` or `NETSTANDARD2_0_OR_GREATER`, when modern `lib/netX.0/` assets also exist, use `net48` (or another supported .NET Framework target from `net462` onward) so the consumer selects `lib/netstandard2.0/`. Never use `netstandard*` as an executable target, and never use a modern `netX.0` target when it selects an asset where the API is absent. For other TFM guards, select a runnable consumer TFM that resolves to the containing asset and confirm that selection from restore or build evidence.

Preserve manual documentation edits. Prefer additive changes, but correct stale or contradictory information so documentation remains accurate.

Preserve working Markdown links, `Related:` references, and historical URL citations during prose rewrites. Remove or replace a URL only after directly verifying that the current destination returns HTTP 404. Timeouts, 403s, rate limits, DNS failures, and other lookup problems are not removal evidence.

Interim scratch artifacts do not belong in the repository working tree. Store assessment queues, project manifests, review reports, captured validator output, progress notes, and one-off helper scripts in temp or session storage instead. New working-tree files are only legitimate when they are the managed `AGENTS.md` block, the active `docfx.json`, the deterministic `skip-compile-allowlist.json` waiver file when one is truly required, or DocFX-authored namespace/type Markdown that maps to a real public namespace or type. Everything else is blocking cleanup work, not a documentation deliverable. The validator auto-detects generic-arity type families (such as `MutableTuple`1`..`MutableTuple`N`) and skips redundant sibling examples from the public API surface alone, so no family-skip manifest is ever written into the repository.

Skip markers are waivers, not fixes. A skip marker only suppresses compilation when it both existed before the current run and matches an entry in `.docfx/skip-compile-allowlist.json`. Each allowlist entry must include `diagnosticCode`, `filePath`, `uid` or `symbol`, `reason`, `approval`, and `lifetime` (`temporary` or `permanent`). Newly introduced or unallowlisted skip markers remain fail-level diagnostics and do not permit a completion claim.

Do not emit a final report, audit result, completion summary, or handoff while `summary.canClaimCompletion` is false, `summary.remainingWorkItems` is greater than zero, `summary.remainingGates` is non-empty, `summary.fullVerificationRan` is false, fail-level diagnostics remain, `summary.newlyIntroducedSkipMarkers` is non-zero, or `summary.interimArtifacts` is non-zero. Large queues, many changed files, repetitive next steps, long runtimes, context pressure, session length, task size, or a "stable queue" are not valid stop reasons; the next action must be another remediation batch, a validator rerun, a validator/tooling fix, or a true blocker with exact evidence.

Context pressure is not a completion condition. If the session feels constrained while work remains, continue with a smaller deterministic batch, regenerate deterministic queue state such as `--assessment-queue`, `--project-manifest`, or the active dry-run manifest/review pair, or report a true tooling failure with the exact command, exit code, and output. When naming a queue-state regeneration command, resolve it to a concrete temp/session path instead of leaving `<temp-path>` as a placeholder. Do not stop with phrases like "given context constraints", "best done in a follow-up", "remaining work requires authoring", "this is a massive task", or "I will provide a focused summary". A context-sized handoff while work remains is `FAIL_CONTEXT_HANDOFF_WITH_REMAINING_WORK`; the remediation is to continue with a smaller deterministic batch.

Before completing documentation work, run the relevant verification commands, normally:

```bash
dotnet build
dotnet test
dotnet run --file <resolved-skill-dir>/scripts/docfx.cs -- --repo-root . --build-api-model --validate-samples --verify-docfx-build
```

Codebelt repositories are normally strong-name signed with a `.snk` file in the repository root on the main author's codespace. Preserve and copy that root `.snk` file when building a temporary copy. If the repository or temp copy has no root `.snk`, run build and test verification with `-p:SkipSignAssembly=true`, for example `dotnet build -p:SkipSignAssembly=true` and `dotnet test -p:SkipSignAssembly=true`.

The final DocFX verification must run outside the working tree when possible. The `--verify-docfx-build` option copies the repository to a temp workspace, runs DocFX against the resolved `docfx.json` there, and removes the temp workspace afterward so generated API YAML, manifest files, and site output do not flood git status. Do not call the work complete until the final JSON reports `summary.fullVerificationRan: true`, `summary.canClaimCompletion: true`, `summary.remainingWorkItems: 0`, an empty `summary.remainingGates`, an empty `summary.remainingDiagnosticsByCode`, `summary.newlyIntroducedSkipMarkers: 0`, and `summary.interimArtifacts: 0`.

If a command cannot be run, report the exact limitation or failure instead of claiming the documentation was verified.
<!-- dotnet-docfx-digest:end -->

**Scope: Writing Unit Tests.** The following guidance applies to paths matching "**/*.{cs,csproj}".

# Writing Unit Tests
This document provides instructions for writing unit tests for a project/solution. Please follow these guidelines to ensure consistency and maintainability.

## 1. Base Class

**Always inherit from the `Test` base class** for all unit test classes.  
This ensures consistent setup, teardown, and output handling across all tests.

> Important: Do NOT add `using Xunit.Abstractions`. xUnit v3 no longer exposes that namespace; including it is incorrect and will cause compilation errors. Use the `Codebelt.Extensions.Xunit` Test base class and `using Xunit;` as shown in the examples below. If you need access to test output, rely on the Test base class (which accepts the appropriate output helper) rather than importing `Xunit.Abstractions`.

```csharp
using Codebelt.Extensions.Xunit;
using Xunit;

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

## 2. Test Method Attributes

- Use `[Fact]` for standard unit tests.
- Use `[Theory]` with `[InlineData]` or other data sources for parameterized tests.

## 3. Naming Conventions

- **Test classes**: End with `Test` (e.g., `DateSpanTest`).
- **Test methods**: Use descriptive names that state the expected behavior (e.g., `ShouldReturnTrue_WhenConditionIsMet`).

## 4. Assertions

- Use `Assert` methods from xUnit for all assertions.
- Prefer explicit and expressive assertions (e.g., `Assert.Equal`, `Assert.NotNull`, `Assert.Contains`).

## 5. File and Namespace Organization

- Place test files in the appropriate test project and folder structure.
- Use namespaces that mirror the source code structure. The namespace of a test file MUST match the namespace of the System Under Test (SUT). Do NOT append ".Tests", ".Benchmarks" or similar suffixes to the namespace. Only the assembly/project name should indicate that the file is a test/benchmark (for example: YourProject.Foo.Tests assembly, but namespace YourProject.Foo).
    - Example: If the SUT class is declared as:
        ```csharp
        namespace YourProject.Foo.Bar
        {
                public class Zoo { /* ... */ }
        }
        ```
        then the corresponding unit test class must use the exact same namespace:
        ```csharp
        namespace YourProject.Foo.Bar
        {
                public class ZooTest : Test { /* ... */ }
        }
        ```
    - Do NOT use:
        ```csharp
        namespace YourProject.Foo.Bar.Tests { /* ... */ } // ❌
        namespace YourProject.Foo.Bar.Benchmarks { /* ... */ } // ❌
        ```
 - The unit tests for the YourProject.Foo assembly live in the YourProject.Foo.Tests assembly.
 - The functional tests for the YourProject.Foo assembly live in the YourProject.Foo.FunctionalTests assembly.
 - Test class names end with Test and live in the same namespace as the class being tested, e.g., the unit tests for the Boo class that resides in the YourProject.Foo assembly would be named BooTest and placed in the YourProject.Foo namespace in the YourProject.Foo.Tests assembly.
 - Modify the associated .csproj file to override the root namespace so the compiled namespace matches the SUT. Example:
    ```xml
    <PropertyGroup>
        <RootNamespace>YourProject.Foo</RootNamespace>
    </PropertyGroup>
    ```
- When generating test scaffolding automatically, resolve the SUT's namespace from the source file (or project/assembly metadata) and use that exact namespace in the test file header.

- Notes:
  - This rule ensures type discovery and XML doc links behave consistently and reduces confusion when reading tests.
  - Keep folder structure aligned with the production code layout to make locating SUT <-> test pairs straightforward.

## 6. Example Test

```csharp
using System;
using System.Globalization;
using Codebelt.Extensions.Xunit;
using Xunit;

namespace YourProject
{
    /// <summary>
    /// Tests for the <see cref="DateSpan"/> class.
    /// </summary>
    public class DateSpanTest : Test
    {
        public DateSpanTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Parse_ShouldGetOneMonthOfDifference_UsingIso8601String()
        {
            var start = new DateTime(2021, 3, 5).ToString("O");
            var end = new DateTime(2021, 4, 5).ToString("O");

            var span = DateSpan.Parse(start, end);

            Assert.Equal("0:01:31:00:00:00.0", span.ToString());
            Assert.Equal(0, span.Years);
            Assert.Equal(1, span.Months);
            Assert.Equal(31, span.Days);
            Assert.Equal(0, span.Hours);
            Assert.Equal(0, span.Minutes);
            Assert.Equal(0, span.Seconds);
            Assert.Equal(0, span.Milliseconds);

            Assert.Equal(0.08493150684931507, span.TotalYears);
            Assert.Equal(1, span.TotalMonths);
            Assert.Equal(31, span.TotalDays);
            Assert.Equal(744, span.TotalHours);
            Assert.Equal(44640, span.TotalMinutes);
            Assert.Equal(2678400, span.TotalSeconds);
            Assert.Equal(2678400000, span.TotalMilliseconds);

            Assert.Equal(6, span.GetWeeks());
            Assert.Equal(-1566296493, span.GetHashCode());

            TestOutput.WriteLine(span.ToString());
        }
    }
}
```

## 7. Additional Guidelines

- Keep tests focused and isolated.
- Do not rely on external systems except for xUnit itself and Codebelt.Extensions.Xunit (and derived from this).
- Ensure tests are deterministic and repeatable.

## 8. Test Doubles

- Preferred test doubles include dummies, fakes, stubs and spies if and when the design allows it.
- Under special circumstances, mock can be used (using Moq library).
- Before overriding methods, verify that the method is virtual or abstract; this rule also applies to mocks.
- Never mock IMarshaller; always use a new instance of JsonMarshaller.

## 9. Avoid `InternalsVisibleTo` in Tests

- **Do not** use `InternalsVisibleTo` to access internal types or members from test projects.
- Prefer **indirect testing via public APIs** that depend on the internal implementation (public facades, public extension methods, or other public entry points).

### Preferred Pattern

**Pattern name:** Public Facade Testing (also referred to as *Public API Proxy Testing*)

**Description:**  
Internal classes and methods must be validated by exercising the public API that consumes them. Tests should assert observable behavior exposed by the public surface rather than targeting internal implementation details directly.

### Example Mapping

- **Internal helper:** `DelimitedString` (internal static class)  
- **Public API:** `TestOutputHelperExtensions.WriteLines()` (public extension method)  
- **Test strategy:** Write tests for `WriteLines()` and verify its public behavior. The internal call to `DelimitedString.Create()` is exercised implicitly.

### Benefits

- Avoids exposing internal types to test assemblies.
- Ensures tests reflect real-world usage patterns.
- Maintains strong encapsulation and a clean public API.
- Tests remain resilient to internal refactoring as long as public behavior is preserved.

### When to Apply

- Internal logic is fully exercised through existing public APIs.
- Public entry points provide sufficient coverage of internal code paths.
- The internal implementation exists solely as a helper or utility for public-facing functionality.

## 10. ExcludeFromCodeCoverage Prohibition

**Do not use `ExcludeFromCodeCoverage` attribute on any code.** This includes:

- Test classes or test methods
- Production code
- Configuration code
- Any other code path

### Rationale

- Excluding code from coverage hides gaps and creates false confidence in test completeness.
- If a code path cannot or should not be tested, refactor the code to eliminate that path rather than hiding it from metrics.
- Every executable line should be covered by tests or be genuinely unreachable (dead code to be removed).

### Alternative Approaches

- **Untestable code paths**: Refactor to separate concerns and eliminate the untestable path.
- **External dependencies**: Use test doubles (fakes, stubs, spies) instead of excluding from coverage.
- **Configuration-only code**: Move to configuration files or extract into testable methods.
- **Generated or third-party code**: These should not be in the primary codebase; use NuGet packages or dedicated vendor folders if necessary.

**Scope: Writing Performance Tests.** The following guidance applies to paths matching "tuning/**, **/*Benchmark*.cs".

# Writing Performance Tests
This document provides guidance for writing performance tests (benchmarks) for a project/solution using BenchmarkDotNet. Follow these guidelines to keep benchmarks consistent, readable, and comparable.

## 1. Naming and Placement

- Place micro- and component-benchmarks under the `tuning/` folder or in projects named `*.Benchmarks`.
- Place benchmark files in the appropriate benchmark project and folder structure.
- Use namespaces that mirror the source code structure, e.g. do not suffix with `Benchmarks`.
Namespace rule: DO NOT append `.Benchmarks` to the namespace. Benchmarks must live in the same namespace as the production assembly. Example: if the production assembly uses `namespace YourProject.Security.Cryptography`, the benchmark file should also use:
  ```
  namespace YourProject.Security.Cryptography
  {
      public class Sha512256Benchmark { /* ... */ }
  }
  ```
The class name must end with `Benchmark`, but the namespace must match the assembly (no `.Benchmarks` suffix).
- The benchmarks for the YourProject.Bar assembly live in the YourProject.Bar.Benchmarks assembly.
- Benchmark class names end with Benchmark and live in the same namespace as the class being measured, e.g., the benchmarks for the Zoo class that resides in the YourProject.Bar assembly would be named ZooBenchmark and placed in the YourProject.Bar namespace in the YourProject.Bar.Benchmarks assembly.
- Modify the associated .csproj file to override the root namespace, e.g., <RootNamespace>YourProject.Bar</RootNamespace>.

## 2. Attributes and Configuration

- Use `BenchmarkDotNet` attributes to express intent and collect relevant metrics:
    - `[MemoryDiagnoser]` to capture memory allocations.
    - `[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]` to group related benchmarks.
    - `[Params]` for input sizes or variations to exercise multiple scenarios.
    - `[GlobalSetup]` for one-time initialization that's not part of measured work.
    - `[Benchmark]` on methods representing measured operations; consider `Baseline = true` and `Description` to improve report clarity.
- Keep benchmark configuration minimal and explicit; prefer in-class attributes over large shared configs unless re-used widely.

## 3. Structure and Best Practices

- Keep benchmarks focused: each `Benchmark` method should measure a single logical operation.
- Avoid doing expensive setup work inside a measured method; use `[GlobalSetup]`, `[IterationSetup]`, or cached fields instead.
- Use `Params` to cover micro, mid and macro input sizes (for example: small, medium, large) and verify performance trends across them.
- Use small, deterministic data sets and avoid external systems (network, disk, DB). If external systems are necessary, mark them clearly and do not include them in CI benchmark runs by default.
- Capture results that are meaningful: time, allocations, and if needed custom counters. Prefer `MemoryDiagnoser` and descriptive `Description` values.

## 4. Naming Conventions for Methods

- Method names should be descriptive and indicate the scenario, e.g., `Parse_Short`, `ComputeHash_Large`.
- When comparing implementations, mark one method with `Baseline = true` and use similar names so reports are easy to read.

## 5. Example Benchmark

```csharp
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace YourProject
{
    [MemoryDiagnoser]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    public class SampleOperationBenchmark
    {
        [Params(8, 256, 4096)]
        public int Count { get; set; }

        private byte[] _payload;

        [GlobalSetup]
        public void Setup()
        {
            _payload = new byte[Count];
            // deterministic initialization
        }

        [Benchmark(Baseline = true, Description = "Operation - baseline")]
        public int Operation_Baseline() => SampleOperation.Process(_payload);

        [Benchmark(Description = "Operation - optimized")]
        public int Operation_Optimized() => SampleOperation.ProcessOptimized(_payload);
    }
}
```

## 6. Reporting and CI

- Benchmarks are primarily for local and tuning runs; be cautious about running heavy BenchmarkDotNet workloads in CI. Prefer targeted runs or harnesses for CI where appropriate.
- Keep benchmark projects isolated (e.g., `tuning/*.csproj`) so they don't affect package builds or production artifacts.

## 7. Additional Guidelines

- Keep benchmarks readable and well-documented; add comments explaining non-obvious choices.
- If a benchmark exposes regressions or optimizations, add a short note in the benchmark file referencing the relevant issue or PR.
- For any shared helpers for benchmarking, prefer small utility classes inside the `tuning` projects rather than cross-cutting changes to production code.

For further examples, refer to the benchmark files under the `tuning/` folder.

**Scope: Writing XML documentation.** The following guidance applies to paths matching "**/*.cs".

# Writing XML documentation
This document provides instructions for writing XML documentation.

## 1. Documentation Style

- Use the same documentation style as found throughout the codebase.
- Add XML doc comments to public and protected classes and methods where appropriate.
- Example:

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Collections.Generic;
using Cuemon.Configuration;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.Security
{
    /// <summary>
    /// Represents the base class from which all implementations of hash algorithms and checksums should derive.
    /// </summary>
    /// <typeparam name="TOptions">The type of the configured options.</typeparam>
    /// <seealso cref="ConvertibleOptions"/>
    /// <seealso cref="IConfigurable{TOptions}" />
    /// <seealso cref="IHash" />
    public abstract class Hash<TOptions> : Hash, IConfigurable<TOptions> where TOptions : ConvertibleOptions, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hash{TOptions}"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="ConvertibleOptions" /> which may be configured.</param>
        protected Hash(Action<TOptions> setup)
        {
            Options = Patterns.Configure(setup);
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public TOptions Options { get; }


        /// <summary>
        /// The endian-initializer of this instance.
        /// </summary>
        /// <param name="options">An instance of the configured options.</param>
        protected sealed override void EndianInitializer(EndianOptions options)
        {
            options.ByteOrder = Options.ByteOrder;
        }
    }

    /// <summary>
    /// Represents the base class that defines the public facing structure to expose.
    /// </summary>
    /// <seealso cref="IHash" />
    public abstract class Hash : IHash
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        protected Hash()
        {
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="bool"/>.
        /// </summary>
        /// <param name="input">The <see cref="bool"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(bool input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="byte"/>.
        /// </summary>
        /// <param name="input">The <see cref="byte"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(byte input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="char"/>.
        /// </summary>
        /// <param name="input">The <see cref="char"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(char input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="DateTime"/>.
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(DateTime input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="DBNull"/>.
        /// </summary>
        /// <param name="input">The <see cref="DBNull"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(DBNull input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="decimal"/>.
        /// </summary>
        /// <param name="input">The <see cref="decimal"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(decimal input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="double"/>.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(double input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="short"/>.
        /// </summary>
        /// <param name="input">The <see cref="short"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(short input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="int"/>.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(int input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="long"/>.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(long input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="sbyte"/>.
        /// </summary>
        /// <param name="input">The <see cref="sbyte"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(sbyte input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="float"/>.
        /// </summary>
        /// <param name="input">The <see cref="float"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(float input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="ushort"/>.
        /// </summary>
        /// <param name="input">The <see cref="ushort"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(ushort input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="uint"/>.
        /// </summary>
        /// <param name="input">The <see cref="uint"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(uint input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="ulong"/>.
        /// </summary>
        /// <param name="input">The <see cref="ulong"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(ulong input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="string"/>.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to compute the hash code for.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(string input, Action<EncodingOptions> setup = null)
        {
            return ComputeHash(Convertible.GetBytes(input, setup));
        }

        /// <summary>
        /// Computes the hash value for the specified <see cref="Enum"/>.
        /// </summary>
        /// <param name="input">The <see cref="Enum"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(Enum input)
        {
            return ComputeHash(Convertible.GetBytes(input, EndianInitializer));
        }

        /// <summary>
        /// Computes the hash value for the specified <c>IConvertible[]</c>.
        /// </summary>
        /// <param name="input">The <c>IConvertible[]</c> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(params IConvertible[] input)
        {
            return ComputeHash(Arguments.ToEnumerableOf(input));
        }

        /// <summary>
        /// Computes the hash value for the specified sequence of <see cref="IConvertible"/>.
        /// </summary>
        /// <param name="input">The sequence of <see cref="IConvertible"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(IEnumerable<IConvertible> input)
        {
            return ComputeHash(Convertible.GetBytes(input));
        }

        /// <summary>
        /// Computes the hash value for the specified <c>byte[]</c>.
        /// </summary>
        /// <param name="input">The <c>byte[]</c> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public abstract HashResult ComputeHash(byte[] input);

        /// <summary>
        /// Computes the hash value for the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to compute the hash code for.</param>
        /// <returns>A <see cref="HashResult"/> containing the computed hash code of the specified <paramref name="input"/>.</returns>
        public virtual HashResult ComputeHash(Stream input)
        {
            return ComputeHash(Patterns.SafeInvoke(() => new MemoryStream(), destination =>
            {
                Decorator.Enclose(input).CopyStream(destination);
                return destination;
            }).ToArray());
        }

        /// <summary>
        /// Defines the initializer that <see cref="Hash{TOptions}"/> must implement.
        /// </summary>
        /// <param name="options">An instance of the configured options.</param>
        protected abstract void EndianInitializer(EndianOptions options);
    }
}

namespace Cuemon.Security
{
    /// <summary>
    /// Configuration options for <see cref="FowlerNollVoHash"/>.
    /// </summary>
    public class FowlerNollVoOptions : ConvertibleOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FowlerNollVoOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="FowlerNollVoOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="EndianOptions.ByteOrder"/></term>
        ///         <description><see cref="Endianness.BigEndian"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Algorithm"/></term>
        ///         <description><see cref="FowlerNollVoAlgorithm.Fnv1a"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public FowlerNollVoOptions()
        {
            Algorithm = FowlerNollVoAlgorithm.Fnv1a;
            ByteOrder = Endianness.BigEndian;
        }

        /// <summary>
        /// Gets or sets the algorithm of the Fowler-Noll-Vo hash function.
        /// </summary>
        /// <value>The algorithm of the Fowler-Noll-Vo hash function.</value>
        public FowlerNollVoAlgorithm Algorithm { get; set; }
    }
}
```
