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

## Official Documentation

- Public API conventions belong in `.docfx/api/namespaces/` and should be treated as the official documentation source for library behavior and naming vocabulary.
- When adding or renaming public APIs, update the relevant namespace page in `.docfx/api/namespaces/` if the change introduces or clarifies a convention.
- Keep internal reasoning, exploratory notes, and agent discussion out of DocFX pages; summarize only stable public guidance.
