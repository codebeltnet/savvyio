using System;
using System.Data;
using System.Threading;
using Cuemon.Extensions;
using Codebelt.Extensions.Xunit;
using Dapper;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Savvyio.Extensions.Dapper
{
    public class DapperQueryOptionsTest : Test
    {
        public DapperQueryOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperOptions_Ensure_Initialization_Defaults()
        {
            var sut = new DapperQueryOptions();

            var cd = (CommandDefinition)sut;

            Assert.Null(sut.CommandText);
            Assert.Equal(CommandFlags.Buffered, sut.CommandFlags);
            Assert.Equal(TimeSpan.FromSeconds(30), sut.CommandTimeout);
            Assert.Equal(CommandType.Text, sut.CommandType);
            Assert.Null(sut.Parameters);
            Assert.Null(sut.Transaction);
            Assert.Equal(default(CancellationToken), sut.CancellationToken);
            Assert.Equal(sut.CommandText, cd.CommandText);
            Assert.Equal(sut.CommandFlags, cd.Flags);
            Assert.Equal(sut.CommandTimeout.TotalSeconds.As<int>(), cd.CommandTimeout);
            Assert.Equal(sut.CommandType, cd.CommandType);
            Assert.Equal(sut.Parameters, cd.Parameters);
            Assert.Equal(sut.Transaction, cd.Transaction);

        }

        [Fact]
        public void DapperOptions_ShouldPreserveCustomValuesInImplicitConversion()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var cts = new CancellationTokenSource();
            var parameters = new { Id = 42 };
            var sut = new DapperQueryOptions
            {
                CommandText = "SELECT 1",
                CommandFlags = CommandFlags.NoCache,
                CommandTimeout = TimeSpan.FromSeconds(12),
                CommandType = CommandType.StoredProcedure,
                Parameters = parameters,
                Transaction = transaction,
                CancellationToken = cts.Token
            };

            var cd = (CommandDefinition)sut;

            Assert.Equal("SELECT 1", cd.CommandText);
            Assert.Equal(CommandFlags.NoCache, cd.Flags);
            Assert.Equal(12, cd.CommandTimeout);
            Assert.Equal(CommandType.StoredProcedure, cd.CommandType);
            Assert.Equal(parameters, cd.Parameters);
            Assert.Equal(transaction, cd.Transaction);
            Assert.Equal(cts.Token, cd.CancellationToken);
        }
    }
}
