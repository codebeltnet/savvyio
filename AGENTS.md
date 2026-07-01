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
- `.github/` — CI/CD workflows, contributing guidelines, Copilot instructions
- Dockerfiles and compose — Test environment setup (LocalStack, test isolation)

## Test Conventions

- Test project names must end with `Tests` (e.g., `{PROJECT_NAME}.Tests`)
- Test classes should inherit from the `Test` base class in `Codebelt.Extensions.Xunit`
- Use `Microsoft.Testing.Platform` as the test runner (`UseMicrosoftTestingPlatformRunner=true`)
- All tests are executable (`OutputType=Exe`)
- Test namespaces must match the SUT namespace (System Under Test), without `.Tests` suffix
- See `.github/copilot-instructions.md` for detailed test writing guidelines

## Build & CI

- Centralized package versions via `Directory.Packages.props`
- Resolve new or updated `Directory.Packages.props` versions from NuGet.org and keep them on the latest stable listed releases
- Centralized build configuration via `Directory.Build.props`
- MinVer for semantic versioning from Git tags
- Strong-name signing is enabled in CI environments (`CI=true`)
- Keep `.github/dependabot.yml` enabled at the repo root so central NuGet package management stays current

## .bot/ Folder

If a `.bot/` folder exists at the root, it contains **confidential, local-only** working material for AI agents — product requirement documents (PRDs), design proposals, agentic loop state, and brainstorming outputs. This folder is gitignored and never committed.

When starting creative or design work (new features, architecture decisions, PRD drafts), use the [brainstorming skill](https://skills.sh/obra/superpowers/brainstorming) and save outputs to `.bot/`. Only move finalized, non-confidential instructions into `AGENTS.md` or `.github/copilot-instructions.md`.

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
