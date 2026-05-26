using System;
using System.Collections.Generic;
using System.Data;
using Codebelt.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataSourceTest : Test
    {
        public DapperDataSourceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [MemberData(nameof(GetDapperDataSourceOptions))]
        public void DapperDataSource_ShouldFailWithArgumentNullException(DapperDataSourceOptions options)
        {
            var sut = options == null
                ? Assert.Throws<ArgumentNullException>(() => new DapperDataSource(options))
                : Assert.Throws<ArgumentException>(() => new DapperDataSource(options));
            TestOutput.WriteLine(sut.ToString());
        }

        private static IEnumerable<object[]> GetDapperDataSourceOptions()
        {
            return new List<object[]>
            {
                new object[] { null },
                new object[] { new DapperDataSourceOptions() }
            };
        }

        [Fact]
        public void DapperDataSource_ShouldFailWithInvalidOperationException()
        {
            var options = new DapperDataSourceOptions()
            {
                ConnectionFactory = () =>
                {
                    var cnn = new SqliteConnection("Data Source=:memory:");
                    cnn.Open();
                    return cnn;
                }
            };
            var sut = new DapperDataSource(options);
            sut.Dispose();

            Assert.Throws<InvalidOperationException>(() => sut.BeginTransaction());
            Assert.True(sut.Disposed);
        }

        [Fact]
        public void DapperDataSource_ShouldExposeConnectionMembers()
        {
            var sut = new DapperDataSource(new DapperDataSourceOptions
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:")
            });

            Assert.Equal(ConnectionState.Open, sut.State);
            Assert.NotNull(sut.CreateCommand());
            Assert.Equal("Data Source=:memory:", sut.ConnectionString);

            using (var transaction = sut.BeginTransaction())
            {
                Assert.NotNull(transaction);
                transaction.Rollback();
            }

            Assert.Throws<NotSupportedException>(() => sut.ChangeDatabase("main"));
            Assert.Equal("main", sut.Database);

            sut.Close();
            Assert.Equal(ConnectionState.Closed, sut.State);

            sut.Open();
            Assert.Equal(ConnectionState.Open, sut.State);
        }

        [Fact]
        public void DapperDataSource_ShouldSupportBeginTransactionWithIsolationLevel()
        {
            var sut = new DapperDataSource(new DapperDataSourceOptions
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:")
            });

            using var transaction = sut.BeginTransaction(IsolationLevel.ReadCommitted);

            Assert.NotNull(transaction);
        }

        [Fact]
        public void DapperDataSource_ShouldExposeConnectionTimeout()
        {
            var sut = new DapperDataSource(new DapperDataSourceOptions
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:")
            });

            var timeout = sut.ConnectionTimeout;

            Assert.True(timeout >= 0);
            TestOutput.WriteLine($"ConnectionTimeout: {timeout}");
        }

        [Fact]
        public void DapperDataSource_ShouldSupportConnectionStringSetter()
        {
            var sut = new DapperDataSource(new DapperDataSourceOptions
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:")
            });

            sut.Close();
            sut.ConnectionString = "Data Source=:memory:";

            Assert.Equal("Data Source=:memory:", sut.ConnectionString);
        }
    }
}
