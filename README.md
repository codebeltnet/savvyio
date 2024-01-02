![Savvy I/O](.nuget/Savvyio.Core/icon.png)

# Savvy I/O

An open-source project (MIT license) that provides a SOLID and clean .NET class library for writing DDD, CQRS and Event Sourcing applications for .NET 8 (LTS), .NET 7 (STS) and .NET 6 (LTS).

![Savvy I/O Flow](https://static.savvyio.net/savvyio.png)

It is, by heart, free, flexible and built to extend and boost your agile codebelt.

## Motivation

Savvy I/O is designed to be intuitive and follows many of the same patterns and practices that was applied to [Cuemon for .NET](https://github.com/gimlichael/Cuemon).

The grand idea and motivation was to remove the complexity normally associated with DDD, CQRS and Event Sourcing.

## State of the Union

All CI and CD integrations are done on [Microsoft Azure DevOps](https://azure.microsoft.com/en-us/services/devops/) and is currently in the process of being tweaked.

All code quality analysis are done by [SonarCloud](https://sonarcloud.io/) and [CodeCov.io](https://codecov.io/).

![License](https://img.shields.io/github/license/codebeltnet/classlib-savvyio) [![Build Status](https://dev.azure.com/codebelt/savvyio/_apis/build/status/codebeltnet.classlib-savvyio?branchName=development)](https://dev.azure.com/codebelt/savvyio/_build/latest?definitionId=2&branchName=development) [![codecov](https://codecov.io/gh/codebeltnet/classlib-savvyio/branch/development/graph/badge.svg)](https://codecov.io/gh/codebeltnet/classlib-savvyio) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=savvyio&metric=coverage)](https://sonarcloud.io/dashboard?id=savvyio) [![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa.svg)](.github/CODE_OF_CONDUCT.md)

## Development Branch

The `development` branch contains the latest (and greatest) version of the code.

All CI builds are pushed to NuGet.org as `preview` releases.

Once tested thoroughly and feature milestone has been reached, the code will be pushed and merged to a new branch; `release`.

> NOTE: Builds from development are preview builds and not to be considered stable.

## Release Branch

The `release` branch contains the next version of Savvy I/O. Here it will be tested again while the next semantic version is being determined.

All CI builds are pushed to NuGet.org as either `alpha`, `beta` or `rc` releases (when deemed fit for purpose).

Lastly, when things are looking all fine and dandy, the code will be pushed and merged to the `main` branch.

> Curious for more information about suffixes? Check out [Package versioning - Pre-release Versions](https://docs.microsoft.com/en-us/nuget/concepts/package-versioning#pre-release-versions) at Microsoft.

## Main Branch

The `main` branch always contains the current `production` ready version of Savvy I/O.

Builds performed from this repository are pushed to NuGet.org as the actual version they represent.

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

This is a list of all NuGet packages from Cuemon for .NET that is publicly available on [NuGet.org](https://www.nuget.org/packages?q=savvyio); the packages here are listed alphabetically and are available in preview-, rc- and production-ready versions.

### 📦 Standalone Packages

Provides a focused API for building various types of modern .NET applications suitable for DDD, CQRS and Event Sourcing.

|Package|vNext|Stable|Downloads|
|:--|:-:|:-:|:-:|
| [Savvyio.Commands](https://www.nuget.org/packages/Savvyio.Commands/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Commands?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Commands?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Commands?color=blueviolet&logo=nuget) |
| [Savvio.Core](https://www.nuget.org/packages/Savvyio.Core/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Core?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Core?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Core?color=blueviolet&logo=nuget) |
| [Savvyio.Domain](https://www.nuget.org/packages/Savvyio.Domain/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Domain?color=blueviolet&logo=nuget) |
| [Savvyio.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Domain.EventSourcing/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Domain.EventSourcing?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Domain.EventSourcing?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Domain.EventSourcing?color=blueviolet&logo=nuget) |
| [Savvyio.EventDriven](https://www.nuget.org/packages/Savvyio.EventDriven/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.EventDriven?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.EventDriven?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.EventDriven?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.Dapper](https://www.nuget.org/packages/Savvyio.Extensions.Dapper/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Dapper?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Dapper?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Dapper?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DapperExtensions](https://www.nuget.org/packages/Savvyio.Extensions.DapperExtensions/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DapperExtensions?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DapperExtensions?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DapperExtensions?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.Dapper](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.Dapper/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.Dapper?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.Dapper?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.Dapper?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.DapperExtensions](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.DapperExtensions/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.DapperExtensions?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.DapperExtensions?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.DapperExtensions?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.Domain](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.Domain/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.Domain?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.EFCore](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.EFCore?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.EFCore?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.EFCore?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.EFCore.Domain](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore.Domain/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.EFCore.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.EFCore.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.EFCore.Domain?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.DependencyInjection.SimpleQueueService](https://www.nuget.org/packages/Savvyio.Extensions.DependencyInjection.SimpleQueueService/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.DependencyInjection.SimpleQueueService?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.DependencyInjection.SimpleQueueService?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.DependencyInjection.SimpleQueueService?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.Dispatchers](https://www.nuget.org/packages/Savvyio.Extensions.Dispatchers/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Dispatchers?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Dispatchers?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Dispatchers?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.EFCore](https://www.nuget.org/packages/Savvyio.Extensions.EFCore/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.EFCore?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.EFCore?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.EFCore?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.EFCore.Domain](https://www.nuget.org/packages/Savvyio.Extensions.EFCore.Domain/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.EFCore.Domain?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.EFCore.Domain?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.EFCore.Domain?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.EFCore.Domain.EventSourcing](https://www.nuget.org/packages/Savvyio.Extensions.EFCore.Domain.EventSourcing/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.EFCore.Domain.EventSourcing?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.EFCore.Domain.EventSourcing?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.EFCore.Domain.EventSourcing?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.Newtonsoft.Json](https://www.nuget.org/packages/Savvyio.Extensions.Newtonsoft.Json/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Newtonsoft.Json?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Newtonsoft.Json?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Newtonsoft.Json?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.SimpleQueueService](https://www.nuget.org/packages/Savvyio.Extensions.SimpleQueueService/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.SimpleQueueService?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.SimpleQueueService?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.SimpleQueueService?color=blueviolet&logo=nuget) |
| [Savvyio.Extensions.Text.Json](https://www.nuget.org/packages/Savvyio.Extensions.Text.Json/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Extensions.Text.Json?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Extensions.Text.Json?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Extensions.Text.Json?color=blueviolet&logo=nuget) |
| [Savvyio.Queries](https://www.nuget.org/packages/Savvyio.Queries/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.Queries?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.Queries?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.Queries?color=blueviolet&logo=nuget) |

### 🏭 Productivity Packages

Provides a convenient set of default API additions for building complete DDD, CQRS and Event Sourcing enabled .NET applications using Microsoft Dependency Injection, Microsoft Entity Framework Core, Dapper and AWS SNS/SQS.

|Package|vNext|Stable|Downloads|
|:--|:-:|:-:|:-:|
| [Savvyio.App](https://www.nuget.org/packages/Savvyio.App/) | ![vNext](https://img.shields.io/nuget/vpre/Savvyio.App?logo=nuget) | ![Stable](https://img.shields.io/nuget/v/Savvyio.App?logo=nuget) | ![Downloads](https://img.shields.io/nuget/dt/Savvyio.App?color=blueviolet&logo=nuget) |
