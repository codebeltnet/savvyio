using System;
using System.Collections.Generic;
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
    }
}
