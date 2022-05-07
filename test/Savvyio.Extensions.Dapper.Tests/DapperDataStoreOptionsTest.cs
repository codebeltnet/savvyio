using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dapper
{
    public class DapperDataStoreOptionsTest : Test
    {
        public DapperDataStoreOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperDataStoreOptions_Ensure_Initialization_Defaults()
        {
            var sut = new DapperDataStoreOptions();

            Assert.Null(sut.ConnectionFactory);
        }
    }
}
