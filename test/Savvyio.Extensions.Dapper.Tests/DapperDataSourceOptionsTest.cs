using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

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
    }
}
