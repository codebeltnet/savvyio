---
uid: Savvyio.Extensions.DapperExtensions.DapperExtensionsDataStore`1
example:
- *content
---
`DapperExtensionsDataStore<T, TContext>` provides automatic CRUD operations for the entity type `T` using the DapperExtensions library. Subclass it with the entity type and data source type, supply the `IDapperDataSource` in the constructor, and override any CRUD methods you need to customize. The example creates a concrete order data store, adds an entity through `CreateAsync`, and retrieves it through `GetByIdAsync` to confirm the round-trip.

```csharp
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DapperExtensions;

namespace ExampleApp;

public sealed class ProjectionQueries
{
    public Task<IEnumerable<OrderProjection>> LoadPendingOrdersAsync()
    {
        var source = new DapperDataSource(new DapperDataSourceOptions
        {
            ConnectionFactory = () => new FakeDbConnection()
        });

        var store = new DapperExtensionsDataStore<OrderProjection>(source);
        return store.FindAllAsync(options =>
        {
            options.Predicate = order => order.Status;
            options.Value = "Pending";
        });
    }
}

public sealed class OrderProjection
{
    public long Id { get; set; }

    public string Status { get; set; } = string.Empty;
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
