![Savvy I/O](.nuget/Savvyio.Core/icon.png)

# Savvy I/O

An open-source project (MIT license) that provides a SOLID and clean .NET class library for writing DDD, CQRS and Event Sourcing applications for .NET 9 (STS) and .NET 8 (LTS).

![Savvy I/O Flow](.assets/savvyio.png)

It is, by heart, free, flexible and built to extend and boost your agile codebelt.

## Motivation

Savvy I/O is designed to be intuitive and follows many of the same patterns and practices that was applied to [Cuemon for .NET](https://github.com/gimlichael/Cuemon).

The grand idea and motivation was to remove the complexity normally associated with DDD, CQRS and Event Sourcing.

## State of the Union

Support for .NET 6 and .NET 7 has been deprecated as these are out of [support](https://endoflife.date/dotnet).

> [!IMPORTANT]
> Version 2.2.0 of Savvy I/O will be the last version to support .NET 7.
> Version 3.0.0 of Savvy I/O will be the last version to support .NET 6.

Full documentation (generated by [DocFx](https://github.com/dotnet/docfx)) located here: https://docs.savvyio.net/

All CI and CD integrations have been migrated away from [Microsoft Azure DevOps](https://azure.microsoft.com/en-us/services/devops/) and now embraces GitHub Actions based on the [Codebelt](https://github.com/codebeltnet) umbrella.

All code quality analysis are done by [SonarCloud](https://sonarcloud.io/) and [CodeCov.io](https://codecov.io/).

![License](https://img.shields.io/github/license/codebeltnet/savvyio) ![Build Status](https://github.com/codebeltnet/savvyio/actions/workflows/pipelines.yml/badge.svg?branch=main) [![codecov](https://codecov.io/gh/codebeltnet/savvyio/graph/badge.svg?token=3U45KRDGK6)](https://codecov.io/gh/codebeltnet/savvyio) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=coverage)](https://sonarcloud.io/dashboard?id=savvyio) [![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](.github/CODE_OF_CONDUCT.md) [![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/codebeltnet/savvyio/badge)](https://scorecard.dev/viewer/?uri=github.com/codebeltnet/savvyio)

Full documentation (generated by [DocFx](https://github.com/dotnet/docfx)) located here: https://docs.savvyio.net/

## Branching Strategy

We have finally moved away from the somewhat dated `git flow` branching strategy, and adapted `trunk` based branching that is more aligned with todays DevSecOps practices.

That means, going forward, only one branch will be maintained; `main`. The previous branches, `development` and `release` is for reference only.

> [!NOTE]
> `main` branch will be a clean slate starting from v3.0.0, meaning no previous commits will be preserved. Previous bad practices is a result of this, and going forward we will use Squash or Rebase before committing new code.

## Tag Versioning

We will continue using semantic versioning and account for [pre-release](https://docs.microsoft.com/en-us/nuget/concepts/package-versioning#pre-release-versions) versions when tagging in git.

### Code Quality Monitoring

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=alert_status)](https://sonarcloud.io/dashboard?id=savvyio) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=savvyio) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=savvyio) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=security_rating)](https://sonarcloud.io/dashboard?id=savvyio)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=ncloc)](https://sonarcloud.io/dashboard?id=savvyio) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=code_smells)](https://sonarcloud.io/dashboard?id=savvyio) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=sqale_index)](https://sonarcloud.io/dashboard?id=savvyio) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=bugs)](https://sonarcloud.io/dashboard?id=savvyio) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=savvyio) [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=savvyio)

# Contributing to Savvy I/O

A big welcome and thank you for considering contributing to Savvy I/O open source project!

Please read more about [contributing to Savvy I/O](.github/CONTRIBUTING.md).

# Code of Conduct

Project maintainers pledge to foster an open and welcoming environment, and ask contributors to do the same.

For more information see our [code of conduct](.github/CODE_OF_CONDUCT.md).

## Links to NuGet packages

This is a list of all NuGet packages from Savvy I/O that is publicly available on [NuGet.org](https://www.nuget.org/packages?q=savvyio); the packages here are listed alphabetically and are available in preview-, rc- and production-ready versions.

### 📦 Standalone Packages

Provides a focused API for building various types of modern .NET applications suitable for DDD, CQRS and Event Sourcing.

|Package|vNext|Stable|Downloads|
|:--|:-:|:-:|:-:|
| <font size="2">[Savvyio.Commands](https://www.nuget.org/packages/Savvyio.Commands/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Commands?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Commands?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Commands?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Commands.Messaging](https://www.nuget.org/packages/Savvyio.Commands.Messaging/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Commands.Messaging?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Commands.Messaging?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Commands.Messaging?color=blueviolet&logo=nuget) |
| <font size="2">[Savvio.Core](https://www.nuget.org/packages/Savvyio.Core/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Core?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Core?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Core?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Domain](https://www.nuget.org/packages/Savvyio.Domain/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Domain?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Domain.EventSourcing/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Domain.EventSourcing?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Domain.EventSourcing?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Domain.EventSourcing?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.EventDriven](https://www.nuget.org/packages/Savvyio.EventDriven/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.EventDriven?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.EventDriven?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.EventDriven?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.EventDriven.Messaging](https://www.nuget.org/packages/Savvyio.EventDriven.Messaging/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.EventDriven.Messaging?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.EventDriven.Messaging?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.EventDriven.Messaging?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.Dapper](https://www.nuget.org/packages/Savvyio.Extensions.Dapper/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Dapper?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Dapper?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Dapper?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DapperExtensions](https://www.nuget.org/packages/Savvyio.Extensions.DapperExtensions/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DapperExtensions?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DapperExtensions?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DapperExtensions?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.Dapper](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.Dapper/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.Dapper?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.Dapper?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.Dapper?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.DapperExtensions](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.DapperExtensions/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.DapperExtensions?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.DapperExtensions?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.DapperExtensions?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.Domain](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.Domain/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.Domain?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.EFCore](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.EFCore?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.EFCore?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.EFCore?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.EFCore.Domain](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore.Domain/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.EFCore.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.EFCore.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.EFCore.Domain?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.QueueStorage](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.QueueStorage/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.QueueStorage?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.QueueStorage?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.QueueStorage?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.DependencyInjection.SimpleQueueService](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.SimpleQueueService/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.SimpleQueueService?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.SimpleQueueService?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.SimpleQueueService?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.Dispatchers](https://www.nuget.org/packages/Savvyio.Extensions.Dispatchers/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Dispatchers?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Dispatchers?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Dispatchers?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.EFCore](https://www.nuget.org/packages/Savvyio.Extensions.EFCore/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.EFCore?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.EFCore?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.EFCore?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.EFCore.Domain](https://www.nuget.org/packages/Savvyio.Extensions.EFCore.Domain/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.EFCore.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.EFCore.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.EFCore.Domain?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.EFCore.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Extensions.EFCore.Domain.EventSourcing/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.EFCore.Domain.EventSourcing?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.EFCore.Domain.EventSourcing?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.EFCore.Domain.EventSourcing?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.Newtonsoft.Json](https://www.nuget.org/packages/Savvyio.Extensions.Newtonsoft.Json/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Newtonsoft.Json?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Newtonsoft.Json?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Newtonsoft.Json?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.QueueStorage](https://www.nuget.org/packages/Savvyio.Extensions.QueueStorage/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.QueueStorage?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.QueueStorage?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.QueueStorage?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.SimpleQueueService](https://www.nuget.org/packages/Savvyio.Extensions.SimpleQueueService/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.SimpleQueueService?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.SimpleQueueService?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.SimpleQueueService?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Extensions.Text.Json](https://www.nuget.org/packages/Savvyio.Extensions.Text.Json/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Text.Json?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Text.Json?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Text.Json?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Messaging](https://www.nuget.org/packages/Savvyio.Messaging/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Messaging?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Messaging?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Messaging?color=blueviolet&logo=nuget) |
| <font size="2">[Savvyio.Queries](https://www.nuget.org/packages/Savvyio.Queries/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Queries?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Queries?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Queries?color=blueviolet&logo=nuget) |

### 🏭 Productivity Packages

Provides a convenient set of default API additions for building complete DDD, CQRS and Event Sourcing enabled .NET applications using Microsoft Dependency Injection, Microsoft Entity Framework Core, Dapper and AWS SNS/SQS.

|Package|vNext|Stable|Downloads|
|:--|:-:|:-:|:-:|
| <font size="2">[Savvyio.App](https://www.nuget.org/packages/Savvyio.App/)</font> | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.App?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.App?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.App?color=blueviolet&logo=nuget) |
