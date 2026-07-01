---
uid: Savvyio.Extensions.DependencyInjection.Dapper.DapperDataSource`1
example:
- *content
---
The DI-registered `DapperDataSource<TContext>` is the concrete data source bound by `AddDapperDataSource`. Resolve it as `IDapperDataSource` to obtain connections for Dapper data store operations. The example registers the data source and resolves it from the built provider to confirm the DI binding.

```csharp
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Dapper;

namespace ExampleApp;

public sealed class MarkerBasedDapperSourceExample
{
    public bool IsResolvedAsMarkedSource()
    {
        var services = new ServiceCollection();
        services.AddDapperDataSource<OrdersMarker>(options => options.ConnectionFactory = () => new FakeDbConnection());

        using var provider = services.BuildServiceProvider();
        var source = provider.GetRequiredService<IDapperDataSource<OrdersMarker>>();

        return source is DapperDataSource<OrdersMarker>;
    }
}

public sealed class OrdersMarker
{
}

public sealed class FakeDbConnection : IDbConnection
{
    private ConnectionState _state;

    public string ConnectionString { get; set; } = "Data Source=fake;";

    public int ConnectionTimeout => 30;

    public string Database => "Fake";

    public ConnectionState State => _state;

    public IDbTransaction BeginTransaction()
    {
        return new FakeDbTransaction(this, IsolationLevel.ReadCommitted);
    }

    public IDbTransaction BeginTransaction(IsolationLevel il)
    {
        return new FakeDbTransaction(this, il);
    }

    public void ChangeDatabase(string databaseName)
    {
    }

    public void Close()
    {
        _state = ConnectionState.Closed;
    }

    public IDbCommand CreateCommand()
    {
        return new FakeDbCommand(this);
    }

    public void Open()
    {
        _state = ConnectionState.Open;
    }

    public void Dispose()
    {
        Close();
    }
}

public sealed class FakeDbTransaction : IDbTransaction
{
    public FakeDbTransaction(IDbConnection connection, IsolationLevel isolationLevel)
    {
        Connection = connection;
        IsolationLevel = isolationLevel;
    }

    public IDbConnection Connection { get; }

    public IsolationLevel IsolationLevel { get; }

    public void Commit()
    {
    }

    public void Rollback()
    {
    }

    public void Dispose()
    {
    }
}

public sealed class FakeDbCommand : IDbCommand
{
    public FakeDbCommand(IDbConnection connection)
    {
        Connection = connection;
        Parameters = new FakeParameterCollection();
    }

    public string CommandText { get; set; } = string.Empty;

    public int CommandTimeout { get; set; }

    public CommandType CommandType { get; set; }

    public IDbConnection Connection { get; set; }

    public IDataParameterCollection Parameters { get; }

    public IDbTransaction? Transaction { get; set; }

    public UpdateRowSource UpdatedRowSource { get; set; }

    public void Cancel()
    {
    }

    public IDbDataParameter CreateParameter()
    {
        throw new System.NotSupportedException();
    }

    public void Dispose()
    {
    }

    public int ExecuteNonQuery()
    {
        throw new System.NotSupportedException();
    }

    public IDataReader ExecuteReader()
    {
        throw new System.NotSupportedException();
    }

    public IDataReader ExecuteReader(CommandBehavior behavior)
    {
        throw new System.NotSupportedException();
    }

    public object ExecuteScalar()
    {
        throw new System.NotSupportedException();
    }

    public void Prepare()
    {
    }
}

public sealed class FakeParameterCollection : List<object>, IDataParameterCollection
{
    public object this[string parameterName]
    {
        get => throw new System.NotSupportedException();
        set => throw new System.NotSupportedException();
    }

    public bool Contains(string parameterName)
    {
        return false;
    }

    public int IndexOf(string parameterName)
    {
        return -1;
    }

    public void RemoveAt(string parameterName)
    {
    }
}
```
