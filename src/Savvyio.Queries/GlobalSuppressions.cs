// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S2326:Unused type parameters should be removed", Justification = "By design; marker interface - inheriting interface requires specification of Request.", Scope = "type", Target = "~T:Savvyio.Queries.IQuery`1")]
