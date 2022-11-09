using System;
using System.Data;
using Cuemon;
using Cuemon.Extensions;
using Microsoft.Extensions.Options;

namespace Savvyio.Extensions.Dapper
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IDapperDataSource"/> interface to support the actual I/O communication with a source of data using Dapper.
    /// </summary>
    /// <seealso cref="Disposable" />
    /// <seealso cref="IDapperDataSource" />
    public class DapperDataSource : Disposable, IDapperDataSource
    {
        private readonly IDbConnection _dbConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataSource"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="DapperDataSourceOptions" /> which need to be configured.</param>
        public DapperDataSource(IOptions<DapperDataSourceOptions> setup)
        {
            Validator.ThrowIfInvalidOptions(setup?.Value, nameof(setup));
            _dbConnection = setup!.Value.ConnectionFactory.Invoke();
            _dbConnection.Open();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperDataSource"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="DapperDataSourceOptions" /> which need to be configured.</param>
        public DapperDataSource(Action<DapperDataSourceOptions> setup) : this(Options.Create(setup.Configure()))
        {
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="M:Cuemon.Disposable.Dispose" /> or <see cref="M:Cuemon.Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="P:Cuemon.Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            return _dbConnection.BeginTransaction();
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        /// <param name="il">One of the <seealso cref="IsolationLevel"/> values.</param>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _dbConnection.BeginTransaction(il);
        }

        /// <summary>
        /// Changes the current database for an open <see langword="Connection" /> object.
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database.</param>
        public void ChangeDatabase(string databaseName)
        {
            _dbConnection.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        public void Close()
        {
            _dbConnection.Close();
        }

        /// <summary>
        /// Creates and returns a Command object associated with the connection.
        /// </summary>
        /// <returns>A Command object associated with the connection.</returns>
        public IDbCommand CreateCommand()
        {
            return _dbConnection.CreateCommand();
        }

        /// <summary>
        /// Opens a database connection with the settings specified by the <see langword="ConnectionString" /> property of the provider-specific Connection object.
        /// </summary>
        public void Open()
        {
            _dbConnection.Open();
        }

        /// <summary>
        /// Gets or sets the string used to open a database.
        /// </summary>
        /// <value>A string containing connection settings.</value>
        public string ConnectionString
        {
            get => _dbConnection.ConnectionString;
            set => _dbConnection.ConnectionString = value;
        }

        /// <summary>
        /// Gets the time to wait (in seconds) while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <value>The time (in seconds) to wait for a connection to open. The default value is 15 seconds.</value>
        public int ConnectionTimeout => _dbConnection.ConnectionTimeout;

        /// <summary>
        /// Gets the name of the current database or the database to be used after a connection is opened.
        /// </summary>
        /// <value>The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.</value>
        public string Database => _dbConnection.Database;

        /// <summary>
        /// Gets the current state of the connection.
        /// </summary>
        /// <value>One of the <see cref="ConnectionState" /> values.</value>
        public ConnectionState State => _dbConnection.State;
    }
}
