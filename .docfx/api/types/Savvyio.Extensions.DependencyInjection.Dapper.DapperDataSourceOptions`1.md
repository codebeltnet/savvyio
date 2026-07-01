---
uid: Savvyio.Extensions.DependencyInjection.Dapper.DapperDataSourceOptions`1
example:
- *content
---
`DapperDataSourceOptions<TContext>` holds the `ConnectionFactory` and `Lifetime` values populated when `AddDapperDataSource` is called. The `ConnectionFactory` property must produce an open database connection. The example configures options with a test factory and shows how the type appears in a complete DI setup.

```csharp
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Savvyio.Extensions.DependencyInjection.Dapper;

namespace ExampleApp;

public sealed class MarkerOptionsExample
{
    public DapperDataSource<OrdersMarker> CreateSource()
    {
        var options = new DapperDataSourceOptions<OrdersMarker>
        {
            ConnectionFactory = () => new FakeDbConnection()
        };

        return new DapperDataSource<OrdersMarker>(options);
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
