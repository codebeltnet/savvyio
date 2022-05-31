using Cuemon.Extensions.Xunit;
using Savvyio.Extensions.EFCore;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Storage
{
    public class EfCoreDataSourceOptionsTest : Test
    {
        public EfCoreDataSourceOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void EfCoreDataSourceOptions_Ensure_Initialization_Defaults()
        {
            var sut = new EfCoreDataSourceOptions();

            Assert.Null(sut.ContextConfigurator);
            Assert.Null(sut.ConventionsConfigurator);
            Assert.Null(sut.ModelConstructor);
        }
    }
}
