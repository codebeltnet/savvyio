using Codebelt.Extensions.Xunit;
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
    }
}
