---
uid: Savvyio.Extensions.Dapper.DapperDataSourceOptions
example:
- *content
---
`DapperDataSourceOptions` holds the `ConnectionFactory` delegate that `DapperDataSource<TContext>` uses to create `IDbConnection` instances. Setting `ConnectionFactory` to a lambda that opens a database connection is the minimum required configuration. The example configures a factory pointing at an in-memory SQLite database and confirms the options object passes validation.

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Savvyio.Extensions.Dapper;

namespace ExampleApp;

public sealed class DapperOptionsExample
{
    public DapperDataSource CreateSource()
    {
        var options = new DapperDataSourceOptions
        {
            ConnectionFactory = () => new FakeDbConnection()
        };

        return new DapperDataSource(options);
    }
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
        throw new NotSupportedException();
    }

    public void Dispose()
    {
    }

    public int ExecuteNonQuery()
    {
        throw new NotSupportedException();
    }

    public IDataReader ExecuteReader()
    {
        throw new NotSupportedException();
    }

    public IDataReader ExecuteReader(CommandBehavior behavior)
    {
        throw new NotSupportedException();
    }

    public object ExecuteScalar()
    {
        throw new NotSupportedException();
    }

    public void Prepare()
    {
    }
}

public sealed class FakeParameterCollection : List<object>, IDataParameterCollection
{
    public object this[string parameterName]
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
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
