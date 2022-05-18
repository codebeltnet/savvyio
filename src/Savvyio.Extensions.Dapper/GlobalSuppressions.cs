// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "By design; Dapper require developers to write there own SQL.", Scope = "member", Target = "~M:Savvyio.Extensions.Dapper.DefaultDapperDataAccessObject`1.CreateAsync(`0,System.Action{Savvyio.Extensions.Dapper.DapperOptions})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "By design; Dapper require developers to write there own SQL.", Scope = "member", Target = "~M:Savvyio.Extensions.Dapper.DefaultDapperDataAccessObject`1.UpdateAsync(`0,System.Action{Savvyio.Extensions.Dapper.DapperOptions})~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "By design; Dapper require developers to write there own SQL.", Scope = "member", Target = "~M:Savvyio.Extensions.Dapper.DefaultDapperDataAccessObject`1.ReadAllAsync(System.Action{Savvyio.Extensions.Dapper.DapperOptions})~System.Threading.Tasks.Task{System.Collections.Generic.IEnumerable{`0}}")]
[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "By design; Dapper require developers to write there own SQL.", Scope = "member", Target = "~M:Savvyio.Extensions.Dapper.DefaultDapperDataAccessObject`1.ReadAsync(System.Action{Savvyio.Extensions.Dapper.DapperOptions})~System.Threading.Tasks.Task{`0}")]
[assembly: SuppressMessage("Critical Code Smell", "S1006:Method overrides should not change parameter defaults", Justification = "By design; Dapper require developers to write there own SQL.", Scope = "member", Target = "~M:Savvyio.Extensions.Dapper.DefaultDapperDataAccessObject`1.DeleteAsync(`0,System.Action{Savvyio.Extensions.Dapper.DapperOptions})~System.Threading.Tasks.Task")]
