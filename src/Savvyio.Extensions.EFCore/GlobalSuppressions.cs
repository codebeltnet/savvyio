// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "By design. No added complexity in this scope.", Scope = "member", Target = "~M:Savvyio.Extensions.EFCore.EfCoreRepository`2.FindAllAsync(System.Linq.Expressions.Expression{System.Func{`0,System.Boolean}},System.Action{Cuemon.Threading.AsyncOptions})~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{`0}}")]
[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "By design; Predicate needs to be set on setup delegate.", Scope = "member", Target = "~M:Savvyio.Extensions.EFCore.EfCoreDataAccessObject`1.ReadAsync(System.Action{Savvyio.Extensions.EFCore.EfCoreOptions{`0}})~System.Threading.Tasks.Task{`0}")]
