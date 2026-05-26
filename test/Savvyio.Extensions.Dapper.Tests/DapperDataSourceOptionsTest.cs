using Codebelt.Extensions.Xunit;
using Microsoft.Data.Sqlite;
using Xunit;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataSourceOptionsTest : Test
    {
        public DapperDataSourceOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperDataSourceOptions_Ensure_Initialization_Defaults()
        {
            var sut = new DapperDataSourceOptions();

            Assert.Null(sut.ConnectionFactory);
        }

        [Fact]
        public void DapperDataSourceOptions_ValidateOptions_ShouldPassWhenConnectionFactoryIsSet()
        {
            var sut = new DapperDataSourceOptions
            {
                ConnectionFactory = () => new SqliteConnection("Data Source=:memory:")
            };

            var exception = Record.Exception(sut.ValidateOptions);

            Assert.Null(exception);
        }
    }
}
