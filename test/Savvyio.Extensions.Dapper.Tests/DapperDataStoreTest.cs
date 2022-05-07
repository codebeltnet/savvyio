using System;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataStoreTest : Test
    {
        public DapperDataStoreTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperDataStore_ShouldFailWithArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DapperDataStore(Options.Create(new DapperDataStoreOptions())));
        }

        [Fact]
        public void DapperDataStore_ShouldFailWithInvalidOperationException()
        {
            var sut = new DapperDataStore(o => o.ConnectionFactory = () =>
            {
                var cnn = new SqliteConnection("Data Source=:memory:");
                cnn.Open();
                return cnn;
            });
            sut.Dispose();

            Assert.Throws<InvalidOperationException>(() => sut.BeginTransaction());
            Assert.True(sut.Disposed);
        }
    }
}
