---
uid: Savvyio.Extensions.DependencyInjection.Dapper.ServiceCollectionExtensions
example:
- *content
---
Wiring Dapper persistence into Savvy I/O requires two sequential registrations: `AddDapperDataSource` to register the connection factory and `IDapperDataSource`, and `AddDapperDataStore` to bind each data store implementation to its service interface. The connection factory lambda must return an IDbConnection instance; the data source must be registered before the data stores. The example builds a provider with both registrations and confirms the data store resolves to the expected concrete type.

```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Savvyio;
using Savvyio.Data;
using Savvyio.Extensions.Dapper;
using Savvyio.Extensions.DependencyInjection;
using Savvyio.Extensions.DependencyInjection.Dapper;

namespace ExampleApp;

public sealed class DapperRegistrationExample
{
    public ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddMarshaller<FakeMarshaller>(p => new FakeMarshaller());
        services.AddDapperDataSource(options => options.ConnectionFactory = () => new FakeDbConnection());
        services.AddDapperDataStore<OrderProjectionStore, OrderProjection>();

        return services.BuildServiceProvider();
    }

    public bool HasExpectedRegistration(ServiceProvider provider)
    {
        var source = provider.GetRequiredService<IDapperDataSource>();
        var store = provider.GetRequiredService<IPersistentDataStore<OrderProjection, DapperQueryOptions>>();

        return source is DapperDataSource && store is OrderProjectionStore;
    }
}

public sealed class OrderProjection
{
    public long Id { get; set; }

    public string Status { get; set; } = string.Empty;
}

public sealed class OrderProjectionStore : DapperDataStore<OrderProjection, DapperQueryOptions>
{
    public OrderProjectionStore(IDapperDataSource source) : base(source)
    {
    }

    public override Task CreateAsync(OrderProjection dto, Action<Cuemon.Threading.AsyncOptions>? setup = null)
    {
        return Task.CompletedTask;
    }

    public override Task UpdateAsync(OrderProjection dto, Action<Cuemon.Threading.AsyncOptions>? setup = null)
    {
        return Task.CompletedTask;
    }

    public override Task<OrderProjection> GetByIdAsync(object id, Action<Cuemon.Threading.AsyncOptions>? setup = null)
    {
        return Task.FromResult(new OrderProjection { Id = Convert.ToInt64(id), Status = "Pending" });
    }

    public override Task<IEnumerable<OrderProjection>> FindAllAsync(Action<DapperQueryOptions>? setup = null)
    {
        return Task.FromResult<IEnumerable<OrderProjection>>(Array.Empty<OrderProjection>());
    }

    public override Task DeleteAsync(OrderProjection dto, Action<Cuemon.Threading.AsyncOptions>? setup = null)
    {
        return Task.CompletedTask;
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

public sealed class FakeMarshaller : IMarshaller
{
    public Stream Serialize<TValue>(TValue value) => Stream.Null;
    public Stream Serialize(object value, Type inputType) => Stream.Null;
    public TValue Deserialize<TValue>(Stream data) => default!;
    public object Deserialize(Stream data, Type returnType) => null!;
}
```
