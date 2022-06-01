// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI.", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Domain.IDeletableRepository`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI.", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Domain.IPersistentRepository`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI.", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Domain.IReadableRepository`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI.", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Domain.IRepository`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI.", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Domain.IWritableRepository`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI.", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Domain.ISearchableRepository`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Data.ISearchableDataStore`3")]
[assembly: SuppressMessage("Major Code Smell", "S2436:Types and methods should not have too many generic parameters", Justification = "By design to support service-to-multiple implementations in MS DI", Scope = "type", Target = "~T:Savvyio.Extensions.DependencyInjection.Data.IPersistentDataStore`3")]
