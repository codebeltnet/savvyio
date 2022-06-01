using System;
using Cuemon.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataSourceTest : Test
    {
        public DapperDataSourceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperDataSource_ShouldFailWithArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DapperDataSource(Options.Create(new DapperDataSourceOptions())));
        }

        [Fact]
        public void DapperDataSource_ShouldFailWithInvalidOperationException()
        {
            var sut = new DapperDataSource(o => o.ConnectionFactory = () =>
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
